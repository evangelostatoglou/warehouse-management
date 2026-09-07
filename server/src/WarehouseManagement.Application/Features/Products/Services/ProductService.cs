using WarehouseManagement.Application.Features.Products.DTOs;

namespace WarehouseManagement.Application.Features.Products.Services;

public class ProductService : IProductService
{
    public Task<IReadOnlyList<ProductResponse>> GetAllAsync()
    {
        IReadOnlyList<ProductResponse> products =
        [
            new ProductResponse
            {
                Id = 1,
                Sku = "MOUSE-001",
                Name = "Wireless Mouse",
                Price = 29.99m,
                IsActive = true
            }
        ];

        return Task.FromResult(products);
    }
}