using CakeBake.Data;
using CakeBake.Enums;
using CakeBake.Interfaces;
using CakeBake.Models;
using CakeBake.ViewModels.Cart;
using CakeBake.ViewModels.Checkout;
using Microsoft.EntityFrameworkCore;

namespace CakeBake.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddToCartAsync(string userId, int productId, int quantity)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    ApplicationUserId = userId
                };

                _context.Carts.Add(cart);

                await _context.SaveChangesAsync();
            }

            var product = await _context.Products
    .FirstOrDefaultAsync(p =>
        p.Id == productId &&
        p.IsAvailable);

            if (product == null)
                return;

            var cartItem = cart.CartItems
                .FirstOrDefault(ci => ci.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity = Math.Min(
    cartItem.Quantity + quantity,
    product.StockQuantity);
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = product.Price
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<CartViewModel> GetCartAsync(string userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == userId);

            if (cart == null)
                return new CartViewModel();

            var model = new CartViewModel();

            model.Items = cart.CartItems
                .Select(ci => new CartItemViewModel
                {
                    ProductId = ci.ProductId,
                    ProductName = ci.Product!.Name,
                    ImageUrl = ci.Product.ImageUrl,
                    UnitPrice = ci.UnitPrice,
                    Quantity = ci.Quantity,
                    StockQuantity = ci.Product.StockQuantity
                })
                .ToList();

            return model;
        }

        public async Task<int> GetCartCountAsync(string userId)
        {
            return await _context.CartItems
                .Include(ci => ci.Cart)
                .Where(ci => ci.Cart!.ApplicationUserId == userId)
                .SumAsync(ci => ci.Quantity);
        }

        public async Task RemoveItemAsync(string userId, int productId)
        {
            var cartItem = await _context.CartItems
                .Include(ci => ci.Cart)
                .FirstOrDefaultAsync(ci =>
                    ci.ProductId == productId &&
                    ci.Cart!.ApplicationUserId == userId);

            if (cartItem == null)
                return;

            _context.CartItems.Remove(cartItem);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateQuantityAsync(string userId, int productId, int quantity)
        {
            var cartItem = await _context.CartItems
                .Include(ci => ci.Cart)
                .FirstOrDefaultAsync(ci =>
                    ci.ProductId == productId &&
                    ci.Cart!.ApplicationUserId == userId);

            if (cartItem == null)
                return;

            if (quantity <= 0)
            {
                _context.CartItems.Remove(cartItem);
            }
            else
            {
                cartItem.Quantity = quantity;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<CheckoutViewModel> GetCheckoutAsync(string userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == userId);

            if (cart == null)
                return new CheckoutViewModel();

            return new CheckoutViewModel
            {
                Items = cart.CartItems.Select(x => new CartItemViewModel
                {
                    ProductId = x.ProductId,
                    ProductName = x.Product!.Name,
                    ImageUrl = x.Product.ImageUrl,
                    Quantity = x.Quantity,
                    UnitPrice = x.Product.Price,
                }).ToList(),

                GrandTotal = cart.CartItems.Sum(x => x.Product!.Price * x.Quantity)
            };
        }

        public async Task<bool> PlaceOrderAsync(string userId, CheckoutViewModel model)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == userId);

            if (cart == null || !cart.CartItems.Any())
                return false;

            var order = new Order
            {
                ApplicationUserId = userId,
                DeliveryAddress = model.DeliveryAddress,
                Notes = model.Notes,
                OrderDate = DateTime.Now,
                Status = OrderStatus.Pending,
                TotalPrice = cart.CartItems.Sum(x => x.Product!.Price * x.Quantity)
            };

            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            foreach (var item in cart.CartItems)
            {
                _context.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product!.Price
                });
            }

            await _context.SaveChangesAsync();

            _context.CartItems.RemoveRange(cart.CartItems);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<int>> GetProductIdsInCartAsync(string userId)
        {
            return await _context.CartItems
                .Include(ci => ci.Cart)
                .Where(ci => ci.Cart!.ApplicationUserId == userId)
                .Select(ci => ci.ProductId)
                .ToListAsync();
        }

        public async Task<Dictionary<int, int>> GetCartQuantitiesAsync(string userId)
        {
            return await _context.CartItems
                .Include(ci => ci.Cart)
                .Where(ci => ci.Cart!.ApplicationUserId == userId)
                .GroupBy(ci => ci.ProductId)
                .ToDictionaryAsync(
                    g => g.Key,
                    g => g.Sum(x => x.Quantity)
                );
        }

        public async Task<int> ChangeQuantityAsync(string userId, int productId, int change)
        {
            var cartItem = await _context.CartItems
             .Include(ci => ci.Cart)
             .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci =>
                ci.ProductId == productId &&
                 ci.Cart!.ApplicationUserId == userId);

            if (cartItem == null)
                return 0;

            cartItem.Quantity += change;

            if (cartItem.Quantity > cartItem.Product!.StockQuantity)
            {
                cartItem.Quantity = cartItem.Product.StockQuantity;
            }
            if (cartItem.Quantity <= 0)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
                return 0;
            }

            await _context.SaveChangesAsync();

            return cartItem.Quantity;
        }

        public async Task<(decimal ItemTotal, decimal GrandTotal)> GetTotalsAsync(string userId, int productId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == userId);

            if (cart == null)
                return (0, 0);

            var item = cart.CartItems.FirstOrDefault(x => x.ProductId == productId);

            decimal itemTotal = 0;

            if (item != null)
                itemTotal = item.Product!.Price * item.Quantity;

            decimal grandTotal = cart.CartItems.Sum(x => x.Product!.Price * x.Quantity);

            return (itemTotal, grandTotal);
        }
    }
}