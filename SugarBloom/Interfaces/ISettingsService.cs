using CakeBake.ViewModels.Settings;

namespace CakeBake.Interfaces
{
    public interface ISettingsService
    {
        Task<ProfileViewModel?> GetProfileAsync(string userId);

        Task<bool> UpdateProfileAsync(string userId, ProfileViewModel model);
    }
}