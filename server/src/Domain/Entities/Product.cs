


using System.Data.Common;

namespace WM.Domain.Entities;

/// <summary>
/// int Id
/// <br/>
/// string Sku
/// <br/>
/// string Name
/// <br/>
/// string? Description
/// <br/>
/// decimal Price
/// <br/>
/// bool IsActive
/// <br/>
/// DateTime CreatedAt
/// </summary>
public class Product
{
    public int Id { get; private set; }

    public string Sku { get; private set; } = null!;

    public string Name { get; private set; } = null!;

    public string? Description { get; private set; }

    public decimal Price { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private Product()
    {
        
    }

    public Product(string sku, string name, string? description, decimal price)
    {
        if (String.IsNullOrWhiteSpace(sku))
        {
            throw new ArgumentException("SKU is required", nameof(sku));
        }
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Product name is required.", nameof(name));
        }

        if (price < 0)
        {
            throw new ArgumentException(
                "Price cannot be negative.",
                nameof(price));
        }

        Sku = sku.Trim();
        Name = name.Trim();
        Description = description?.Trim();
        Price = price;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update( string sku, string name, string? description, decimal price)
    {
        if (string.IsNullOrWhiteSpace(sku))
        {
            throw new ArgumentException("SKU is required.", nameof(sku));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Product name is required.", nameof(name));
        }

        if (price < 0)
        {
            throw new ArgumentException(
                "Price cannot be negative.",
                nameof(price));
        }

        Sku = sku.Trim();
        Name = name.Trim();
        Description = description?.Trim();
        Price = price;
    }



    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }



}



