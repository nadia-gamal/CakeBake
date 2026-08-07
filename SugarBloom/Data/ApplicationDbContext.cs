using CakeBake.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CakeBake.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<OtpCode> OtpCodes { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<ContactMessage> ContactMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>()
                   .HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<OrderItem>()
                   .HasOne(oi => oi.Order)
                   .WithMany(o => o.OrderItems)
                   .HasForeignKey(oi => oi.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<OrderItem>()
                   .HasOne(oi => oi.Product)
                   .WithMany(p => p.OrderItems)
                   .HasForeignKey(oi => oi.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Order>()
                   .HasOne(o => o.ApplicationUser)
                   .WithMany(u => u.Orders)
                   .HasForeignKey(o => o.ApplicationUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Cart>()
       .HasOne(c => c.ApplicationUser)
       .WithOne(u => u.Cart)
       .HasForeignKey<Cart>(c => c.ApplicationUserId)
       .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CartItem>()
       .HasOne(ci => ci.Cart)
       .WithMany(c => c.CartItems)
       .HasForeignKey(ci => ci.CartId)
       .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CartItem>()
       .HasOne(ci => ci.Product)
       .WithMany(p => p.CartItems)
       .HasForeignKey(ci => ci.ProductId)
       .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<OtpCode>()
       .HasOne(o => o.User)
       .WithMany(u => u.OtpCodes)
       .HasForeignKey(o => o.UserId)
       .OnDelete(DeleteBehavior.Cascade);


        }
    }
}