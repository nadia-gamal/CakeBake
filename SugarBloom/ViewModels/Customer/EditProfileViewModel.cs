using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CakeBake.ViewModels.Customer
{
    public class EditProfileViewModel
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? CurrentImage { get; set; }

        public IFormFile? Image { get; set; }

        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "New password must be at least 6 characters.")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}