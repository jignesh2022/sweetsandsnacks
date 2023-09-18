using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.QuickInfo;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SweetsAndSnacks.Data;
using SweetsAndSnacks.Models;
using SweetsAndSnacks.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;

namespace SweetsAndSnacks.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DBContext _context;

        public HomeController(ILogger<HomeController> logger, DBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Request.Cookies["sid"] == null)
            {
                HttpContext.Response.Cookies.Append("sid", Guid.NewGuid().ToString(), new CookieOptions()
                {
                    Secure = true,
                    HttpOnly = true,
                    SameSite = SameSiteMode.Lax
                });
            }
            return _context.Products != null ?
                          View(await _context.Products.ToListAsync()) :
                          Problem("Entity set 'DBContext.Products'  is null.");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string searchstr, int? pageNumber)
        {
            if (_context.Products == null)
            {
                return Problem("Product list is dempty.");
            }

            if (string.IsNullOrWhiteSpace(searchstr))
            {
                return RedirectToAction("Index");
            }

            pageNumber = pageNumber ?? 1;

            var products = from p in _context.Products
                         select p;
            var result = _context.Products.Where(x => x.ProductName.Contains(searchstr));
            ViewData["searchstr"] = searchstr;

            //int pageSize = 3;
            //return View(await PaginatedList<Product>.CreateAsync(result.AsNoTracking(), pageNumber ?? 1, pageSize));

            return View(await result.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Cart()
        {
            string? sid = null;

            if (HttpContext.Request.Cookies["sid"] != null)
            {
                sid = HttpContext.Request.Cookies["sid"].ToString();
            }
            else
            {
                return RedirectToAction("Index");
            }

            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.Sid == sid);
            if(cart == null)
            {
                return View();
            }
            

            List<CartItemDto> list = JsonConvert.DeserializeObject<List<CartItemDto>>(cart.CartItemList);

            ViewData["CartCount"] = list.Count.ToString();
            ViewData["total"] = list.Sum(i => i.Price).ToString("N");
            return View(list);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cart(int pid,int qty)
        {
            if (pid <= 0 || qty <= 0)
            {
                return RedirectToAction("Index");
            }

            string? sid = null;

            if (HttpContext.Request.Cookies["sid"] != null)
            {
                sid = HttpContext.Request?.Cookies?["sid"]?.ToString();
            }
            else  
            {
                return RedirectToAction("Index");
            }

            List<CartItemDto>? items = null;

            Product? product = null;
            if (pid > 0) 
            {
                product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == pid);
                if (product == null)
                {
                    return RedirectToAction("Index");
                }
            }

            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.Sid == sid);


            if (cart != null)
            {
                items = JsonConvert.DeserializeObject<List<CartItemDto>>(cart.CartItemList);
                if(qty > product.Stock)
                {
                    ModelState.AddModelError("","Required quantity is more than stock.");
                    return RedirectToAction("Index");
                }
                items.Add(new CartItemDto() {
                    ProductName = product.ProductName,
                    ProductImageName = product.ImageName,
                    Quantity = qty,
                    Price = Utilities.Helper.GetItemPrice(qty,product.PricingQuantity,product.Pricing),
                    Pricing = product.Pricing,
                    PricingQuantity = product.PricingQuantity,
                    PricingQuantityUnit = Enum.GetName(typeof(QuantityUnit), product.QuantityUnit)
                });
                cart.CartItemList = JsonConvert.SerializeObject(items);
                _context.Carts.Update(cart);
                product.Stock = product.Stock - qty;
                _context.Products.Update(product);
                _context.SaveChanges();

                
            }            
            else 
            {
                
                if(qty > product.Stock)
                {
                    ModelState.AddModelError("", "Required quantity is more than stock.");
                    return RedirectToAction("Index");
                }
                items = new List<CartItemDto>();
                items.Add(new CartItemDto()
                {
                    ProductName = product.ProductName,
                    ProductImageName = product.ImageName,
                    Quantity = qty,
                    Price = Utilities.Helper.GetItemPrice(qty, product.PricingQuantity, product.Pricing),
                    Pricing = product.Pricing,
                    PricingQuantity = product.PricingQuantity,
                    PricingQuantityUnit = Enum.GetName(typeof(QuantityUnit), product.QuantityUnit)
                });
                await _context.Carts.AddAsync(new Cart()
                {
                    Sid = sid,
                    CartItemList = JsonConvert.SerializeObject(items),                    
                    CreatedAt = DateTime.Now
                });
                product.Stock = product.Stock - qty;
                _context.Products.Update(product);
                _context.SaveChanges();
            }

            return RedirectToAction("Cart");
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCartItem(string id)
        {
            string sid = null;
            if (HttpContext.Request.Cookies["sid"] != null)
            {
                sid = HttpContext.Request?.Cookies?["sid"]?.ToString();
            }
            else
            {
                return RedirectToAction("Index");
            }

            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.Sid == sid);
            var items = JsonConvert.DeserializeObject<List<CartItemDto>>(cart.CartItemList);
            var itemToBeRemoved = items.Where(i => i.Id == id).FirstOrDefault();
            var product = await _context.Products.Where(i => i.ProductName == itemToBeRemoved.ProductName).FirstOrDefaultAsync();
            product.Stock = product.Stock + itemToBeRemoved.Quantity;
            if (itemToBeRemoved != null)
            {
                items.Remove(itemToBeRemoved);

                cart.CartItemList = JsonConvert.SerializeObject(items);
                _context.Carts.Update(cart);
                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                return Json("success");
            }

            return Json("fail");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            /*var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var exceptionPath = exceptionDetails.Path;
            var exceptionMessage = exceptionDetails.Error.Message;
            var exceptionStackTrace = exceptionDetails.Error.StackTrace;

            _logger.LogError(String.Concat(exceptionMessage, " ReqId:",
                    Activity.Current?.Id ?? HttpContext.TraceIdentifier, "\r\n",
                    exceptionStackTrace.Substring(0, exceptionStackTrace.IndexOf(":line", 0) + 12)));*/

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}