using CakeBake.Data;
using CakeBake.Interfaces;
using CakeBake.Models;
using CakeBake.ViewModels.Categories;
using Microsoft.EntityFrameworkCore;

namespace CakeBake.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> CreateAsync(CreateCategoryViewModel model)
        {
            bool exists = await _context.Categories.AnyAsync(c =>
                c.Name.ToLower() == model.Name.Trim().ToLower());

            if (exists)
                return false;

            var category = new Category
            {
                Name = model.Name.Trim()
            };

            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(EditCategoryViewModel model)
        {
            var category = await _context.Categories.FindAsync(model.Id);

            if (category == null)
                return false;

            bool exists = await _context.Categories.AnyAsync(c =>
                c.Id != model.Id &&
                c.Name.ToLower() == model.Name.Trim().ToLower());

            if (exists)
                return false;

            category.Name = model.Name.Trim();

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return false;

            if (category.Products.Any())
                return false;

            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}