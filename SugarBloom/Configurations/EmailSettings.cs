using System.ComponentModel.DataAnnotations;

namespace CakeBake.Configurations
{
    public class EmailSettings
    {
        [Required]
        public string SenderName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string SenderEmail { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string SmtpServer { get; set; } = string.Empty;

        [Range(1, 65535)]
        public int Port { get; set; }
    }
}