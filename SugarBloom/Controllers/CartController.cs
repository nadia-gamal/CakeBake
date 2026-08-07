using CakeBake.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CakeBake.Models;

namespace CakeBake.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(
            ICartService cartService,
            UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User)!;

            var model = await _cartService.GetCartAsync(userId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            var userId = _userManager.GetUserId(User)!;

            await _cartService.AddToCartAsync(userId, productId, quantity);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAjax(int productId, int quantity = 1)
        {
            var userId = _userManager.GetUserId(User)!;

            await _cartService.AddToCartAsync(userId, productId, quantity);

            var cartCount = await _cartService.GetCartCountAsync(userId);

            return Json(new
            {
                success = true,
                cartCount
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAjax(int productId, int quantity)
        {
            var userId = _userManager.GetUserId(User)!;

            await _cartService.UpdateQuantityAsync(userId, productId, quantity);

            var cart = await _cartService.GetCartAsync(userId);

            var item = cart.Items.FirstOrDefault(x => x.ProductId == productId);

            return Json(new
            {
                success = true,
                cartCount = await _cartService.GetCartCountAsync(userId),
                itemTotal = item?.TotalPrice ?? 0,
                grandTotal = cart.GrandTotal
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveAjax(int productId)
        {
            var userId = _userManager.GetUserId(User)!;

            await _cartService.RemoveItemAsync(userId, productId);

            var cart = await _cartService.GetCartAsync(userId);

            return Json(new
            {
                success = true,
                cartCount = await _cartService.GetCartCountAsync(userId),
                grandTotal = cart.GrandTotal
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int productId)
        {
            var userId = _userManager.GetUserId(User)!;

            await _cartService.RemoveItemAsync(userId, productId);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int productId, int quantity)
        {
            var userId = _userManager.GetUserId(User)!;

            await _cartService.UpdateQuantityAsync(userId, productId, quantity);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeQuantity(int productId, int change)
        {
            var userId = _userManager.GetUserId(User)!;

            var quantity = await _cartService.ChangeQuantityAsync(userId, productId, change);

            var cartCount = await _cartService.GetCartCountAsync(userId);

            return Json(new
            {
                success = true,
                quantity,
                cartCount
            });
        }

    }
}

