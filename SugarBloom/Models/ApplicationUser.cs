using Microsoft.AspNetCore.Identity;

namespace CakeBake.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
       
        public ICollection<OtpCode> OtpCodes { get; set; } = new List<OtpCode>();

        public Cart? Cart { get; set; }

        public string? ImageUrl { get; set; }

    }
}