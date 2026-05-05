using Microsoft.EntityFrameworkCore;
using RetailPOS.API.Models;

namespace RetailPOS.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }
        public DbSet<StockHistory> StockHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User constraints
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Username).IsUnique();
            });

            // Product constraints
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasIndex(e => e.Barcode).IsUnique().HasFilter("Barcode IS NOT NULL");
                entity.Property(e => e.SellingPrice).HasDefaultValue(0);
                entity.Property(e => e.CostPrice).HasDefaultValue(0);
            });

            // Sale → SaleItem relationship
            modelBuilder.Entity<SaleItem>(entity =>
            {
                entity.HasOne(si => si.Sale)
                      .WithMany(s => s.SaleItems)
                      .HasForeignKey(si => si.SaleId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(si => si.Product)
                      .WithMany()
                      .HasForeignKey(si => si.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ✅ Seed default admin user (STATIC VALUES ONLY)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Email = "admin@retailpos.local",
                    PasswordHash = "$2a$11$8KxPvZ9Z9Z9Z9Z9Z9Z9Z9OeYqX7X7X7X7X7X7X7X7X7X7X7X7X7X7",
                    Role = "Admin",
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        } 
    } 
} 