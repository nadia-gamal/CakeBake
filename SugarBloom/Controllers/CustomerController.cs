using CakeBake.Constants;
using CakeBake.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CakeBake.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _context.Users
                .Include(u => u.Orders)
                .Where(u => _context.UserRoles.Any(ur =>
                    ur.UserId == u.Id &&
                    _context.Roles.Any(r =>
                        r.Id == ur.RoleId &&
                        r.Name == Roles.Customer)))
                .OrderBy(u => u.FullName)
                .ToListAsync();

            return View(customers);
        }
    }
}