using CakeBake.Data;
using CakeBake.Interfaces;
using CakeBake.Models;
using CakeBake.ViewModels;

namespace CakeBake.Services
{
    public class ContactService : IContactService
    {
        private readonly ApplicationDbContext _context;

        public ContactService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendMessageAsync(ContactViewModel model)
        {
            ContactMessage message = new ContactMessage
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Subject = model.Subject,
                Message = model.Message
            };

            _context.ContactMessages.Add(message);

            await _context.SaveChangesAsync();
        }
    }
}