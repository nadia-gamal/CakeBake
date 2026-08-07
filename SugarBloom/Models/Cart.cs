using System.ComponentModel.DataAnnotations;

namespace CakeBake.Models
{
    public class Cart
    {
        public int Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;

        // Navigation Property
        public ApplicationUser? ApplicationUser { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Property
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}