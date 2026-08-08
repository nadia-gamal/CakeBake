using CakeBake.Enums;

namespace CakeBake.ViewModels.Orders
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalPrice { get; set; }

        public OrderStatus Status { get; set; }

        public string DeliveryAddress { get; set; } = string.Empty;

        public int PhoneNumber { get; set; }

        public string? Notes { get; set; }

        public List<OrderItemViewModel> Items { get; set; } = new();
    }

    public class OrderItemViewModel
    {
        public string ProductName { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Total =>
            Quantity * UnitPrice;
    }
}