using CakeBake.Models;
using CakeBake.ViewModels.Shop;

namespace CakeBake.ViewModels.Home
{
    public class HomeViewModel
    {
        // Products
        public List<ShopProductViewModel> FeaturedProducts { get; set; } = new();
        public List<Product> BestSellers { get; set; } = new();
        public List<Product> NewArrivals { get; set; } = new();

        // Categories
        public List<Category> Categories { get; set; } = new();
    }
}