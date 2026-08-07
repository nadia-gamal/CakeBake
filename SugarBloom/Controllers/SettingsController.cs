using CakeBake.Constants;
using CakeBake.Interfaces;
using CakeBake.Models;
using CakeBake.ViewModels.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CakeBake.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class SettingsController : Controller
    {
        private readonly ISettingsService _settingsService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SettingsController(
            ISettingsService settingsService,
            UserManager<ApplicationUser> userManager)
        {
            _settingsService = settingsService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = _userManager.GetUserId(User);

            var model = await _settingsService.GetProfileAsync(userId);

            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {

            if (!ModelState.IsValid)
            {
                TempData["Error"] = string.Join(" | ",
                    ModelState.Values
                              .SelectMany(v => v.Errors)
                              .Select(e => e.ErrorMessage));

                return View(model);
            }

            var userId = _userManager.GetUserId(User)!;

            var user = await _userManager.FindByIdAsync(userId);

            if (user != null && !await _userManager.IsInRoleAsync(user, Roles.Admin))
            {
                await _userManager.AddToRoleAsync(user, Roles.Admin);
            }

            
            var result = await _settingsService.UpdateProfileAsync(userId, model);

            TempData["Success"] = "Profile updated successfully.";

            return RedirectToAction(nameof(Profile));
        }

        public IActionResult Security()
        {
            return View();
        }

        


    }
}