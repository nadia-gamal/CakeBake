using CakeBake.Data;
using CakeBake.Interfaces;
using CakeBake.ViewModels.Orders;
using Microsoft.EntityFrameworkCore;

namespace CakeBake.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderViewModel>> GetMyOrdersAsync(string userId)
        {
            var orders = await _context.Orders

                .Include(o => o.OrderItems)

                .ThenInclude(i => i.Product)

                .Where(o => o.ApplicationUserId == userId)

                .OrderByDescending(o => o.OrderDate)

                .ToListAsync();

            return orders.Select(o => new OrderViewModel
            {
                Id = o.Id,

                OrderDate = o.OrderDate,

                Status = o.Status,

                TotalPrice = o.TotalPrice,

                DeliveryAddress = o.DeliveryAddress,

                Notes = o.Notes,

                Items = o.OrderItems.Select(i => new OrderItemViewModel
                {
                    ProductName = i.Product!.Name,

                    ImageUrl = i.Product.ImageUrl,

                    Quantity = i.Quantity,

                    UnitPrice = i.UnitPrice

                }).ToList()

            }).ToList();
        }
    }
}