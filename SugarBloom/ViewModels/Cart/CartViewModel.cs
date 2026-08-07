namespace CakeBake.ViewModels.Cart
{
    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new();

        public decimal GrandTotal => Items.Sum(i => i.TotalPrice);

        public int TotalItems => Items.Sum(i => i.Quantity);
    }
}