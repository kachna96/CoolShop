using System;
using System.ComponentModel.DataAnnotations;

namespace CoolShop.WebApi.Domain.Entities;

/// <summary>
/// Product table
/// </summary>
public class Product
{
    /// <summary>
    /// Id of a product
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of a product
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Link to an image
    /// </summary>
    [Required]
    public Uri ImageUri { get; set; }

    /// <summary>
    /// Product price
    /// </summary>
    [Required]
    public decimal Price { get; set; }

    /// <summary>
    /// Optional product description
    /// </summary>
    public string Description { get; set; }
}
