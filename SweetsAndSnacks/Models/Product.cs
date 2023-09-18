using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SweetsAndSnacks.Models
{
    [Index(nameof(ProductName),IsUnique = true)]
    public class Product
    {
        
        public int ProductId { get; set; }
        
        [Required]
        [StringLength(50)]        
        public string ProductName { get; set; } = string.Empty;
                
        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;

        [DataType("tinyint")]
        public Category Category { get; set; }

        [DataType("decimal(18,2)")]
        public decimal Pricing { get; set; }

        public int PricingQuantity { get; set; }

        [DataType("tinyint")]
        public QuantityUnit QuantityUnit { get; set; }

        public int Stock { get; set; } = 0;
                
        public string ImageName { get; set; } = "no-image.jpg";

        public string? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedAt { get; set; }

        [NotMapped]
        public IFormFile? Photo { get; set; }
        
    }

    public enum Category
    {
        Sweet=1,
        Snack
    }

    public enum QuantityUnit
    {
        Grams = 1,
        Piece
    }

}
