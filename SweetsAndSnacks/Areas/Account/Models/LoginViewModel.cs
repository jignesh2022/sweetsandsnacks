using Microsoft.Build.Framework;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SweetsAndSnacks.Areas.Account.Models
{
    public class LoginViewModel
    {
        [System.ComponentModel.DataAnnotations.Required]
        [EmailAddress]
        public string Email { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Remember Me")]
        public bool RememberMe { get; set; }
    }
}
