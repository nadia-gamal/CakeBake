using CakeBake.Constants;
using CakeBake.Data;
using CakeBake.Models;
using CakeBake.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CakeBake.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var model = new DashboardViewModel
            {
                ProductsCount = _context.Products.Count(),
                CategoriesCount = _context.Categories.Count(),
                UsersCount = _userManager.Users.Count(),
                OrdersCount = _context.Orders.Count(),

                OutOfStockCount = _context.Products.Count(x => x.StockQuantity == 0),
                PendingOrders = _context.Orders.Count(x => x.Status == CakeBake.Enums.OrderStatus.Pending),
            };

            return View(model);
        }
    }
}