using CakeBake.Interfaces;
using CakeBake.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CakeBake.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _contactService.SendMessageAsync(model);

            TempData["Success"] = "Your message has been sent successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}