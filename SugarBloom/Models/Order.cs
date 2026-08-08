using CakeBake.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CakeBake.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [Required]
        [StringLength(250)]
        public string DeliveryAddress { get; set; } = string.Empty;

        [Required]
        public int PhoneNumber { get; set; } 

        [StringLength(500)]
        public string? Notes { get; set; }

        // Foreign Key
        public string ApplicationUserId { get; set; } = string.Empty;

        // Navigation Property
        public ApplicationUser? ApplicationUser { get; set; }

        // Navigation Property
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}