using CakeBake.ViewModels.Orders;

namespace CakeBake.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderViewModel>> GetMyOrdersAsync(string userId);
    }
}