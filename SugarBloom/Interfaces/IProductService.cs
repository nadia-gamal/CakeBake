using CakeBake.Models;
using CakeBake.ViewModels.Products;

namespace CakeBake.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetAllAsync();

        Task<Product?> GetByIdAsync(int id);

        Task<bool> CreateAsync(CreateProductViewModel model);

        Task<bool> UpdateAsync(EditProductViewModel model);

        Task DeleteAsync(int id);

        Task<(List<Product> Products, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        int? categoryId = null);

        Task<List<Product>> GetRelatedProductsAsync(int productId, int categoryId);
    }
}