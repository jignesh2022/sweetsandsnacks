using SweetsAndSnacks.Models;

namespace SweetsAndSnacks.Utilities
{
    public static class Helper
    {
        public static decimal GetItemPrice(decimal qty, decimal pricingQty, decimal pricing)
        {
            decimal price = 0;
            price = (qty / pricingQty) * pricing;
            return price;
        }

        public static Order GetOrderTotal(Order order)
        {            
            decimal total = 0;
            decimal taxamt = 0;
            foreach (var item in order.OrderItems)
            {
                item.Price = 0;// GetItemPrice(item.Quantity, item.Product);
                total += item.Price;
                
            }

            if (order.TaxRatePercentage > 0)
            {
                taxamt = (order.TaxRatePercentage / 100) * total;
                total += taxamt;
            }
            else
            {
                order.TaxRatePercentage = 0;
            }

            if (order.DeliveryCharge > 0)
            {
                total += total + order.DeliveryCharge;
            }
            else
            {
                order.DeliveryCharge = 0;
            }

            order.TaxAmount = taxamt;
            order.Total = total;
            
            return order;
        }
    }
}
