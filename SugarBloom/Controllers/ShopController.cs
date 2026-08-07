using Microsoft.AspNetCore.Identity;
using CakeBake.Models;
using CakeBake.ViewModels.Shop;
using CakeBake.Interfaces;
using CakeBake.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CakeBake.Controllers
{
    public class ShopController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ShopController(
         ICategoryService categoryService,
         IProductService productService,
         ICartService cartService,
         UserManager<ApplicationUser> userManager)
        {
            _categoryService = categoryService;
            _productService = productService;
            _cartService = cartService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string? search, int? categoryId, int page = 1)
        {
            const int pageSize = 6;

            var result = await _productService.GetPagedAsync(page, pageSize, search, categoryId);

            Dictionary<int, int> cartQuantities = new();

            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = _userManager.GetUserId(User)!;

                cartQuantities = await _cartService.GetCartQuantitiesAsync(userId);
            }

            var model = new ShopViewModel
            {
                Categories = await _categoryService.GetAllAsync(),

                Products = result.Products.Select(p => new ShopProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    CategoryName = p.Category?.Name,
                    CartQuantity = cartQuantities.ContainsKey(p.Id)
                    ? cartQuantities[p.Id]
                     : 0,
                    IsAvailable = p.IsAvailable,
                    StockQuantity = p.StockQuantity
                }),

                CurrentPage = page,

                TotalPages = (int)Math.Ceiling(result.TotalCount / (double)pageSize),

                Search = search,

                CategoryId = categoryId
            };

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            var relatedProducts = await _productService.GetRelatedProductsAsync(
             product.Id,
             product.CategoryId 
            );

            int cartQuantity = 0;

            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = _userManager.GetUserId(User)!;

                var quantities = await _cartService.GetCartQuantitiesAsync(userId);

                if (quantities.ContainsKey(id))
                    cartQuantity = quantities[id];
            }

            var model = new ProductDetailsViewModel
            {
                Product = product,
                RelatedProducts = relatedProducts,
                CartQuantity = cartQuantity
            };

            return View(model);
        }
    }
}