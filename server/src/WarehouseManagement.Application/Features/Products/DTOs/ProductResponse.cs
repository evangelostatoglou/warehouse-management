namespace WarehouseManagement.Application.Features.Products.DTOs;

public class ProductResponse
{
    public int Id { get; init; }

    public string Sku { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public decimal Price { get; init; }

    public bool IsActive { get; init; }
}