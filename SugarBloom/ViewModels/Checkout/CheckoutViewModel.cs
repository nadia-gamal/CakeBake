using CakeBake.ViewModels.Cart;
using System.ComponentModel.DataAnnotations;

namespace CakeBake.ViewModels.Checkout
{
    public class CheckoutViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new();

        public decimal GrandTotal { get; set; }

        [Required]
        [Display(Name = "Delivery Address")]
        public string DeliveryAddress { get; set; } = string.Empty;

        [Display(Name = "Notes")]
        public string? Notes { get; set; }
    }
}