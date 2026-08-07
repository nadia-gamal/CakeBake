namespace CakeBake.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Phone { get; set; }

        public string Subject { get; set; } = null!;

        public string Message { get; set; } = null!;

        public DateTime SentAt { get; set; } = DateTime.Now;
    }
}