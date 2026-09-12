

namespace WM.Domain.Entities;

/// <summary>
/// int Id
/// <br/>
/// String Sku
/// <br/>
/// String Name
/// <br/>
/// String? Description
/// <br/>
/// decimal Price
/// <br/>
/// int ReorderLevel
/// <br/>
/// bool IsActive
/// <br/>
/// DateTime CreatedAt
/// </summary>
public class Product
{
    public int Id { get; private set; }

    public String Sku { get; private set; } = null!;

    public String Name { get; private set; } = null!;

    public String? Description { get; private set; }

    public decimal Price { get; private set; }

    public int ReorderLevel { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private Product(){}

    public Product(String sku, String name, String? description, decimal price, int reorderLevel)
    {
        if (String.IsNullOrWhiteSpace(sku))
            throw new ArgumentException("SKU is required", nameof(sku));

        if (String.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required",nameof(name));

        if (price < 0)
            throw new ArgumentException("Price cannot be negative.",nameof(price));

        if (reorderLevel < 0)
            throw new ArgumentException("Reorder level cannot be negative.", nameof(reorderLevel));

        Sku = sku.Trim();
        Name = name.Trim();
        Description = description?.Trim();
        Price = price;
        ReorderLevel = reorderLevel;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update( String sku, String name, String? description, decimal price, int reorderLevel)
    {
        if (String.IsNullOrWhiteSpace(sku))
            throw new ArgumentException("SKU is required.", nameof(sku));

        if (String.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name is required.", nameof(name));

        if (price < 0)
        throw new ArgumentException("Price cannot be negative.", nameof(price));

        if (reorderLevel < 0)
            throw new ArgumentException("Reorder level cannot be negative.", nameof(reorderLevel));


        Sku = sku.Trim();
        Name = name.Trim();
        Description = description?.Trim();
        Price = price;
        ReorderLevel = reorderLevel;
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



