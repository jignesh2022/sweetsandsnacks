namespace SweetsAndSnacks.Models
{
    public class CartItemDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ProductName { get; set; }
        public string ProductImageName { get; set; }
        public int ProductStock { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Pricing { get; set; }
        public int PricingQuantity { get; set; }
        public string PricingQuantityUnit { get; set; }
    }
        
}
