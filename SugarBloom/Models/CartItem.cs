using System.ComponentModel.DataAnnotations.Schema;

namespace CakeBake.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        // Foreign Key
        public int CartId { get; set; }

        // Navigation Property
        public Cart? Cart { get; set; }

        // Foreign Key
        public int ProductId { get; set; }

        // Navigation Property
        public Product? Product { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
    }
}