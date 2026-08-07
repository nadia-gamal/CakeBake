using CakeBake.ViewModels.Cart;
using CakeBake.ViewModels.Checkout;

namespace CakeBake.Interfaces
{
    public interface ICartService
    {
        Task AddToCartAsync(string userId, int productId, int quantity);

        Task<CartViewModel> GetCartAsync(string userId);

        Task RemoveItemAsync(string userId, int productId);

        Task UpdateQuantityAsync(string userId, int productId, int quantity);

        Task<CheckoutViewModel> GetCheckoutAsync(string userId);

        Task<bool> PlaceOrderAsync(string userId, CheckoutViewModel model);

        Task<List<int>> GetProductIdsInCartAsync(string userId);

        Task<int> GetCartCountAsync(string userId);

        Task<Dictionary<int, int>> GetCartQuantitiesAsync(string userId);

        Task<int> ChangeQuantityAsync(string userId, int productId, int change);

        Task<(decimal ItemTotal, decimal GrandTotal)> GetTotalsAsync(string userId, int productId);
    }
}