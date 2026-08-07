using CakeBake.Data;
using CakeBake.Interfaces;
using CakeBake.Models;
using CakeBake.ViewModels.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CakeBake.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SettingsService(
    ApplicationDbContext context,
    ICloudinaryService cloudinaryService,
    UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
            _userManager = userManager;
        }
        public async Task<ProfileViewModel?> GetProfileAsync(string userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return null;

            return new ProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email!,
                
                CurrentImage = user.ImageUrl
            };
        }

        public async Task<bool> UpdateProfileAsync(string userId, ProfileViewModel model)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return false;

            user.FullName = model.FullName.Trim();

            user.Email = model.Email.Trim();

            user.UserName = model.Email.Trim();

            if (model.Image != null)
            {
                user.ImageUrl = await _cloudinaryService.UploadImageAsync(model.Image);
            }

            bool wantsPasswordChange =
                !string.IsNullOrWhiteSpace(model.CurrentPassword) ||
                !string.IsNullOrWhiteSpace(model.NewPassword) ||
                !string.IsNullOrWhiteSpace(model.ConfirmPassword);

            if (wantsPasswordChange)
            {
                if (string.IsNullOrWhiteSpace(model.CurrentPassword) ||
                    string.IsNullOrWhiteSpace(model.NewPassword) ||
                    string.IsNullOrWhiteSpace(model.ConfirmPassword))
                {
                    return false;
                }

                var passwordResult = await _userManager.ChangePasswordAsync(
                    user,
                    model.CurrentPassword,
                    model.NewPassword);

                if (!passwordResult.Succeeded)
                    return false;
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(" | ",
                    result.Errors.Select(e => e.Description)));
            }

            return true;
        }
    }
}