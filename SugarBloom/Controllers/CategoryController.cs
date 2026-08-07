using CakeBake.Constants;
using CakeBake.Interfaces;
using CakeBake.ViewModels.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CakeBake.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync();

            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool created = await _categoryService.CreateAsync(model);

            if (!created)
            {
                ModelState.AddModelError("", "Category already exists.");
                return View(model);
            }

            TempData["Success"] = "Category created successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
                return NotFound();

            var model = new EditCategoryViewModel
            {
                Id = category.Id,
                Name = category.Name
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool updated = await _categoryService.UpdateAsync(model);

            if (!updated)
            {
                ModelState.AddModelError("", "Category already exists.");
                return View(model);
            }

            TempData["Success"] = "Category updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _categoryService.DeleteAsync(id);

            if (!deleted)
            {
                TempData["Error"] =
                    "Cannot delete category because it contains products.";

                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] =
                "Category deleted successfully.";

            return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(Index));
        }
    }
}