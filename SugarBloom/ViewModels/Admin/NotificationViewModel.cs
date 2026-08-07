namespace CakeBake.ViewModels.Admin
{
    public class NotificationViewModel
    {
        public int OrderId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; }
    }
}