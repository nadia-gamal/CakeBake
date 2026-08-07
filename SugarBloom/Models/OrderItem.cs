using System.ComponentModel.DataAnnotations.Schema;

namespace CakeBake.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        // Foreign Key
        public int OrderId { get; set; }

        // Navigation Property
        public Order? Order { get; set; }

        // Foreign Key
        public int ProductId { get; set; }

        // Navigation Property
        public Product? Product { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
    }
}