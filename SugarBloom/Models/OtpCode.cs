using System.ComponentModel.DataAnnotations;

namespace CakeBake.Models
{
    public class OtpCode
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(6)]
        public string OTP { get; set; } = string.Empty;

        [Required]
        public DateTime ExpireAt { get; set; }

        public bool IsUsed { get; set; } = false;

        [Required]
        public string UserId { get; set; } = string.Empty;

        // Navigation Property
        public ApplicationUser User { get; set; } = null!;
    }
}