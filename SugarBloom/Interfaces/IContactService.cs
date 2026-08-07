using CakeBake.ViewModels;

namespace CakeBake.Interfaces
{
    public interface IContactService
    {
        Task SendMessageAsync(ContactViewModel model);
    }
}