using CakeBake.Interfaces;
using CakeBake.Models;
using CakeBake.ViewModels.Checkout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CakeBake.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckoutController(
            ICartService cartService,
            UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User)!;

            var model = await _cartService.GetCheckoutAsync(userId);

            if (!model.Items.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var checkout = await _cartService.GetCheckoutAsync(
                    _userManager.GetUserId(User)!);

                checkout.DeliveryAddress = model.DeliveryAddress;
                checkout.Notes = model.Notes;

                return View("Index", checkout);
            }

            var result = await _cartService.PlaceOrderAsync(
                _userManager.GetUserId(User)!,
                model);

            if (!result)
            {
                TempData["Error"] = "Your cart is empty.";

                return RedirectToAction(nameof(Index));
            }


            return RedirectToAction(nameof(Success));
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}