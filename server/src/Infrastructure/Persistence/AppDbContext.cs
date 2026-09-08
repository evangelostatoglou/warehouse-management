using Microsoft.EntityFrameworkCore;
using WM.Domain.Entities;

namespace WM.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(product =>{
            product.HasKey(p => p.Id);
            product.Property(p => p.Id).ValueGeneratedOnAdd();

            product.Property(p => p.Sku).IsRequired().HasMaxLength(50);
            product.HasIndex(p => p.Sku).IsUnique();

            product.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

            product.Property(p => p.Description)
                .HasMaxLength(1000);

            product.Property(p => p.Price)
                .HasPrecision(12, 2);

            product.ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_Products_Price_NonNegative",
                    "\"Price\" >= 0");
            });

            product.Property(p => p.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            product.Property(p => p.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });












    }
}
