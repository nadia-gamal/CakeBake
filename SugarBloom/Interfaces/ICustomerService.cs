using CakeBake.ViewModels.Customer;
using Microsoft.AspNetCore.Identity;

namespace CakeBake.Interfaces
{
    public interface ICustomerService
    {
        Task<EditProfileViewModel?> GetProfileAsync(string userId);

        Task<IdentityResult> UpdateProfileAsync(string userId, EditProfileViewModel model);
    }
}