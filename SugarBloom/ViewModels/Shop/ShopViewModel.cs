using CakeBake.Models;
using CakeBake.ViewModels.Shop;

namespace CakeBake.ViewModels
{
    public class ShopViewModel
    {
        public int? CategoryId { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<ShopProductViewModel> Products { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public string? Search { get; set; }
    }
}