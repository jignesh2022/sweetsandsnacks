using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SweetsAndSnacks.Areas.Account.Models;
using SweetsAndSnacks.Models;

namespace SweetsAndSnacks.Areas.Account.Controllers
{
    [Area("account")]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly ILogger<HomeController> _logger;
        private IHttpContextAccessor _accessor;

        public HomeController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signinManager,
            ILogger<HomeController> logger,
            IHttpContextAccessor accessor)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _logger = logger;
            _accessor = accessor;
        }
        
        [HttpGet]
        public  IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]        
        public async Task<IActionResult> Login(LoginViewModel lvm, string? returnUrl)
        {
            //throw new Exception("test exception");
            if (ModelState.IsValid)
            {
                var result = await _signinManager.PasswordSignInAsync(
                    lvm.Email, lvm.Password, lvm.RememberMe, false);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"{lvm.Email} logged in from {_accessor.HttpContext?.Connection.RemoteIpAddress}.");
                    if (returnUrl != null)
                    {
                        return LocalRedirect(returnUrl); //important to avoid open redirect attack
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { Area = "" });
                    }
                }
            }

            ModelState.AddModelError("", "Login failed. Please try again.");
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel rvm)
        {
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = rvm.Email,
                    Email = rvm.Email
                };
                var result = await _userManager.CreateAsync(user, rvm.Password);
                if (result.Succeeded)
                {
                    await _signinManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home", new { Area = "" });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(rvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signinManager.SignOutAsync();
            return RedirectToAction("Index", "Home",new { Area = "" });
        }

        [AcceptVerbs("Get","Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {            
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return Json(true);
            }
            return Json($"Email '{email}' is already in use.");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
