using CakeBake.Data;
using CakeBake.Interfaces;
using CakeBake.Models;
using CakeBake.ViewModels.Customer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CakeBake.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ApplicationDbContext _context;

        public CustomerService(
            UserManager<ApplicationUser> userManager,
            ICloudinaryService cloudinaryService,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _cloudinaryService = cloudinaryService;
            _context = context;
        }

        public async Task<EditProfileViewModel?> GetProfileAsync(string userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                return null;

            return new EditProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email!,
                CurrentImage = user.ImageUrl
            };
        }

        public async Task<IdentityResult> UpdateProfileAsync(string userId, EditProfileViewModel model)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return IdentityResult.Failed(
                    new IdentityError
                    {
                        Description = "User not found."
                    });
            }

            user.FullName = model.FullName.Trim();

            if (model.Image != null)
            {
                user.ImageUrl =
                    await _cloudinaryService.UploadImageAsync(model.Image);
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
                    return IdentityResult.Failed(
                        new IdentityError
                        {
                            Description = "Please fill in all password fields."
                        });
                }

                if (model.NewPassword != model.ConfirmPassword)
                {
                    return IdentityResult.Failed(
                        new IdentityError
                        {
                            Description = "New password and confirmation do not match."
                        });
                }

                var passwordResult = await _userManager.ChangePasswordAsync(
                    user,
                    model.CurrentPassword,
                    model.NewPassword);

                if (!passwordResult.Succeeded)
                    return passwordResult;
            }

            var result = await _userManager.UpdateAsync(user);

            return result;
        }
    }
}