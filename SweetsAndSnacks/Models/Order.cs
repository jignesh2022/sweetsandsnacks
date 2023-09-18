using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SweetsAndSnacks.Models
{
    public class Order
    {        
        public string OrderId { get; set; }       
        
        
        [Range(0, 100)]
        [DataType("decimal(2,2)")]
        public decimal TaxRatePercentage { get; set; } = 0;

        [DataType("decimal(18,2)")]
        public decimal TaxAmount { get; set; } = 0;

        public int DeliveryCharge { get; set; } = 0;

        [DataType("decimal(18,2)")]
        public decimal Total { get; set; }       
        

        [DataType("tinyint")]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        public string? DeliveryAddress { get; set; }

        public DateTime CreatedAt { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }

    public enum OrderStatus
    {
        Pending = 1,
        Completed,
        Cancelled
    }
}
