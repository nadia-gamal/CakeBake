using CakeBake.Models;
using CakeBake.ViewModels.Categories;

namespace CakeBake.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllAsync();

        Task<Category?> GetByIdAsync(int id);

        Task<bool> CreateAsync(CreateCategoryViewModel model);

        Task<bool> UpdateAsync(EditCategoryViewModel model);

        Task<bool> DeleteAsync(int id);
    }
}