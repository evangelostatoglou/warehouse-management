using WarehouseManagement.Application.Features.Products.DTOs;

namespace WarehouseManagement.Application.Features.Products.Services;

public interface IProductService
{
    Task<IReadOnlyList<ProductResponse>> GetAllAsync();
}