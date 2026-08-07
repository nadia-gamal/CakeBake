using CakeBake.Constants;
using CakeBake.Data;
using CakeBake.Interfaces;
using CakeBake.Models;
using CakeBake.ViewModels.Home;
using CakeBake.ViewModels.Shop;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ICartService _cartService;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(
        ApplicationDbContext context,
        ICartService cartService,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _cartService = cartService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var latestProducts = await _context.Products
            .OrderByDescending(x => x.Id)
            .Take(12)
            .ToListAsync();

        var cartQuantities = new Dictionary<int, int>();

        if (User.Identity!.IsAuthenticated)
        {
            var userId = _userManager.GetUserId(User)!;

            cartQuantities = await _cartService.GetCartQuantitiesAsync(userId);
        }

        var model = new HomeViewModel
        {
            FeaturedProducts = latestProducts
                .OrderBy(x => Guid.NewGuid())
                .Take(3)
                .Select(p => new ShopProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    CategoryName = p.Category?.Name,
                    CartQuantity = cartQuantities.ContainsKey(p.Id)
                     ? cartQuantities[p.Id]
                       : 0,
                       IsAvailable = p.IsAvailable
                })
                .ToList()
        };

        return View(model);
    }

    public async Task<IActionResult> Start()
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null && await _userManager.IsInRoleAsync(user, Roles.Admin))
            {
                return RedirectToAction("Index", "Admin");
            }
        }

        return RedirectToAction("Index", "Home");
    }
}