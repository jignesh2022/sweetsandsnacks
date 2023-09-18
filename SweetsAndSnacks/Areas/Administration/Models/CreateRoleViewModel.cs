using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SweetsAndSnacks.Areas.Administration.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
