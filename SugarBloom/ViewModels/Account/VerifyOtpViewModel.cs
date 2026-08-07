using System.ComponentModel.DataAnnotations;

namespace CakeBake.ViewModels.Account
{
    public class VerifyOtpViewModel
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Verification Code")]
        public string OTP { get; set; } = string.Empty;
    }
}