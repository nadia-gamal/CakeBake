namespace CakeBake.ViewModels.Shop
{
    public class ShopProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public string? CategoryName { get; set; }

        public int CartQuantity { get; set; }

        public bool IsAvailable { get; set; }

        public int StockQuantity { get; set; }
    }
}