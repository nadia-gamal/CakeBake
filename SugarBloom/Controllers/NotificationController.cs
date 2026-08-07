using CakeBake.Constants;
using CakeBake.Data;
using CakeBake.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CakeBake.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class NotificationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotificationController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var notifications = await _context.Orders
                .Include(o => o.ApplicationUser)
                .OrderByDescending(o => o.OrderDate)
                .Take(20)
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
    }
}