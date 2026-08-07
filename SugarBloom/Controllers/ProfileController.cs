using CakeBake.Constants;
using CakeBake.Interfaces;
using CakeBake.Models;
using CakeBake.ViewModels.Customer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CakeBake.Controllers
{
    [Authorize(Roles = Roles.Customer)]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICustomerService _customerService;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            ICustomerService customerService)
        {
            _userManager = userManager;
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User)!;

            var model = await _customerService.GetProfileAsync(userId);

            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(EditProfileViewModel model)
        {
            var userId = _userManager.GetUserId(User)!;

            if (!ModelState.IsValid)
            {
                var profile = await _customerService.GetProfileAsync(userId);

                model.CurrentImage = profile?.CurrentImage;
                model.Email = profile?.Email ?? "";

                return View(model);
            }

            var result = await _customerService.UpdateProfileAsync(userId, model);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    if (error.Description.Contains("password", StringComparison.OrdinalIgnoreCase))
                    {
                        ModelState.AddModelError(nameof(model.CurrentPassword), error.Description);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                var profile = await _customerService.GetProfileAsync(userId);

                model.CurrentImage = profile?.CurrentImage;
                model.Email = profile?.Email ?? "";

                return View(model);
            }

            TempData["Success"] = "Profile updated successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}