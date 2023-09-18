using System.ComponentModel.DataAnnotations;

namespace SweetsAndSnacks.Areas.Administration.Models
{
    public class EditRoleViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; } = new List<string>();
    }
}
