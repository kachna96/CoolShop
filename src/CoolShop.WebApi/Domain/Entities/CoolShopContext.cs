using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;

namespace CoolShop.WebApi.Domain.Entities;

/// <summary>
/// DbContext for a Database
/// </summary>
public class CoolShopContext : DbContext
{
    /// <summary>
    /// Produts table
    /// </summary>
    public DbSet<Product> Products { get; set; }

    /// <inheritdoc/>
    public CoolShopContext(DbContextOptions<CoolShopContext> options)
        : base(options)
    {

    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Guard.Against.Null(modelBuilder, nameof(modelBuilder));

        modelBuilder.Entity<Product>()
            .Property(x => x.Price)
            .HasColumnType("decimal")
            .HasPrecision(18, 2);
    }
}
