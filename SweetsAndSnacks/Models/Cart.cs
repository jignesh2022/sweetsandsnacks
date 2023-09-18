using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SweetsAndSnacks.Models
{
    //temporary table to store cart items and moved to order table. Records will be deleted periodically.
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Sid { get; set; }
        [DataType("varchar(max)")]
        public string CartItemList { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
