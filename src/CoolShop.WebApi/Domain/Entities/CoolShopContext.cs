using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;

namespace CoolShop.WebApi.Domain.Entities;

public class CoolShopContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public CoolShopContext(DbContextOptions<CoolShopContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Guard.Against.Null(modelBuilder, nameof(modelBuilder));

        modelBuilder.Entity<Product>()
            .Property(x => x.Price)
            .HasColumnType("decimal")
            .HasPrecision(18, 2);
    }
}
