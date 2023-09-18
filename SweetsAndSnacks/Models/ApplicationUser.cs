using Microsoft.AspNetCore.Identity;

namespace SweetsAndSnacks.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Pincode { get; set; }
    }
}
