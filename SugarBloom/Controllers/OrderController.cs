using CakeBake.Constants;
using CakeBake.Data;
using CakeBake.Interfaces;
using CakeBake.Models;
using CakeBake.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CakeBake.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public OrderController(
            IOrderService orderService,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _orderService = orderService;
            _userManager = userManager;
            _context = context;
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Index()
        {
            var notifications = await _context.Orders
                .Include(o => o.ApplicationUser)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new NotificationViewModel
                {
                    OrderId = o.Id,
                    CustomerName = o.ApplicationUser!.FullName,
                    Status = o.Status.ToString(),
                    OrderDate = o.OrderDate
                })
                .ToListAsync();

            return View(notifications);
        }

        public async Task<IActionResult> MyOrders()
        {
            var userId = _userManager.GetUserId(User)!;

            var model = await _orderService.GetMyOrdersAsync(userId);

            return View(model);
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders
                .Include(o => o.ApplicationUser)
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            return View(order);
        }
    }
}