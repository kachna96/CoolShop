using System;
using System.ComponentModel.DataAnnotations;

namespace CoolShop.WebApi.Domain.Entities;

public class Product
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public Uri ImageUri { get; set; }

    [Required]
    public decimal Price { get; set; }

    public string Description { get; set; }
}
