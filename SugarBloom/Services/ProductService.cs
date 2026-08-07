using CakeBake.Data;
using CakeBake.Interfaces;
using CakeBake.Models;
using CakeBake.ViewModels.Products;
using Microsoft.EntityFrameworkCore;

namespace CakeBake.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICloudinaryService _cloudinaryService;

        public ProductService(
            ApplicationDbContext context,
            ICloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<(List<Product> Products, int TotalCount)> GetPagedAsync(
          int page,
          int pageSize,
          string? search = null,
          int? categoryId = null)
        {
            IQueryable<Product> query = _context.Products
            .Include(p => p.Category);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var words = search
                    .Trim()
                    .ToLower()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                foreach (var word in words)
                {
                    query = query.Where(p =>
                        p.Name.ToLower().Contains(word));
                }
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            query = query.OrderBy(p => p.Name);

            var totalCount = await query.CountAsync();

            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (products, totalCount);
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> CreateAsync(CreateProductViewModel model)
        {
            bool exists = await _context.Products.AnyAsync(p =>
                p.Name.ToLower() == model.Name.Trim().ToLower());

            if (exists)
                return false;

            string? imageUrl = null;

            if (model.Image != null)
            {
                imageUrl = await _cloudinaryService
                    .UploadImageAsync(model.Image);
            }

            var product = new Product
            {
                Name = model.Name.Trim(),
                Description = model.Description.Trim(),
                Price = model.Price,
                StockQuantity = model.StockQuantity,
                CategoryId = model.CategoryId,
                IsAvailable = model.StockQuantity > 0,
                ImageUrl = imageUrl
            };

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(EditProductViewModel model)
        {
            var product = await _context.Products.FindAsync(model.Id);

            if (product == null)
                return false;

            bool exists = await _context.Products.AnyAsync(p =>
                p.Id != model.Id &&
                p.Name.ToLower() == model.Name.Trim().ToLower());

            if (exists)
                return false;

            if (model.Image != null)
            {
                product.ImageUrl =
                    await _cloudinaryService.UploadImageAsync(model.Image);
            }

            product.Name = model.Name.Trim();
            product.Description = model.Description.Trim();
            product.Price = model.Price;
            product.StockQuantity = model.StockQuantity;
            product.CategoryId = model.CategoryId;
            product.IsAvailable = model.StockQuantity > 0;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return;

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetRelatedProductsAsync(int productId, int categoryId)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p =>
                    p.CategoryId == categoryId &&
                    p.Id != productId)
                .OrderBy(x => Guid.NewGuid())
                .Take(3)
                .ToListAsync();
        }
    }
}