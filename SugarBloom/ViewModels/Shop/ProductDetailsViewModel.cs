using CakeBake.Models;

namespace CakeBake.ViewModels.Shop
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; } = null!;

        public List<Product> RelatedProducts { get; set; } = new();

        public int CartQuantity { get; set; } = 1;
    }
}