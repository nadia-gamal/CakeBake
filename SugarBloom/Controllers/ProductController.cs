using CakeBake.Constants;
using CakeBake.Interfaces;
using CakeBake.ViewModels.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CakeBake.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(
            IProductService productService,
            ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(int? categoryId, bool? isAvailable)
        {
            var products = await _productService.GetAllAsync();
            var categories = await _categoryService.GetAllAsync();


            if (categoryId.HasValue)
            {
                products = products
                    .Where(p => p.CategoryId == categoryId.Value)
                    .ToList();
            }

            if (isAvailable.HasValue)
            {
                products = products
                    .Where(p => p.IsAvailable == isAvailable.Value)
                    .ToList();
            }


            ViewBag.Categories = categories;
            ViewBag.SelectedCategory = categoryId;
            ViewBag.SelectedStatus = isAvailable;

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateProductViewModel();

            await LoadCategories(model);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategories(model);
                return View(model);
            }

            bool created = await _productService.CreateAsync(model);

            if (!created)
            {
                ModelState.AddModelError("", "Product already exists.");

                await LoadCategories(model);

                return View(model);
            }

            TempData["SuccessMessage"] = "Product created successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            var model = new EditProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CurrentImage = product.ImageUrl
            };

            await LoadCategories(model);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategories(model);
                return View(model);
            }

            bool updated = await _productService.UpdateAsync(model);

            if (!updated)
            {
                ModelState.AddModelError("", "Product already exists.");

                await LoadCategories(model);

                return View(model);
            }

            TempData["SuccessMessage"] = "Product updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);

            TempData["SuccessMessage"] = "Product deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadCategories(CreateProductViewModel model)
        {
            var categories = await _categoryService.GetAllAsync();

            model.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });
        }

        private async Task LoadCategories(EditProductViewModel model)
        {
            var categories = await _categoryService.GetAllAsync();

            model.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });
        }
    }
}