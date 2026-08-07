using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CakeBake.ViewModels.Products
{
    public class CreateProductViewModel
    {
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(150,
            MinimumLength = 2,
            ErrorMessage = "Product name must be between 2 and 150 characters.")]
        [Display(Name = "Product Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000,
            MinimumLength = 10,
            ErrorMessage = "Description must be between 10 and 1000 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 100000,
            ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Display(Name = "Stock Quantity")]
        [Range(0, 100000,
            ErrorMessage = "Stock quantity cannot be negative.")]
        public int StockQuantity { get; set; }

        [Display(Name = "Product Image")]
        public IFormFile? Image { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
            = new List<SelectListItem>();
    }
}