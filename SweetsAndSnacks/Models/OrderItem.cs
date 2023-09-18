using SweetsAndSnacks.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SweetsAndSnacks.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
                
        public int Quantity { get; set; }

        [DataType("decimal(18,2)")]
        public decimal Price { get; set; }
        
        public string OrderId { get; set; }
        public Order Order { get; set; }
                
        public int ProductId { get; set; }
        public Product Product { get; set; }

    }
}
