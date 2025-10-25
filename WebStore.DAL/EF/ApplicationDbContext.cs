using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebStore.Model;

namespace WebStore.DAL.EF
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Address> Addresses { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Invoice> Invoices { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderProduct> OrderProducts { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductStock> ProductStocks { get; set; } = null!;
        public DbSet<StationaryStore> StationaryStores { get; set; } = null!;
        public DbSet<StationaryStoreEmployee> StationaryStoreEmployees { get; set; } = null!;
        public DbSet<Supplier> Suppliers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId });

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);
                
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.BillingAddress) // Jeden Customer ma jeden BillingAddress
                .WithMany() // Address nie ma kolekcji "klienci od tego adresu"
                .OnDelete(DeleteBehavior.Restrict); // Zabraniamy kaskadowego usuwania

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.ShippingAddress) // Jeden Customer ma jeden ShippingAddress
                .WithMany() // Address nie ma kolekcji "klienci od tego adresu"
                .OnDelete(DeleteBehavior.Restrict); // Zabraniamy kaskadowego usuwania
            
            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Usunięcie Order usuwa OrderProduct

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Product)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(op => op.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // NIE pozwala usunąć Product, jeśli jest w OrderProduct
        }
    }
}