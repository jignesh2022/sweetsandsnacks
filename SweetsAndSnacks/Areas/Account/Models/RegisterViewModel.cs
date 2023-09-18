using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace SweetsAndSnacks.Areas.Account.Models
{
    public class RegisterViewModel
    {
        [System.ComponentModel.DataAnnotations.Required]
        [EmailAddress]
        [Remote(action: "IsEmailInUse", controller: "Home")]
        public string Email { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]        
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

    }
}
