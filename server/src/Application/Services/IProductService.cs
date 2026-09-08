using WM.Application.DTOs;

namespace WM.Application.Services;

public interface IProductService
{
    Task<IReadOnlyList<GetProductResponse>> GetAllProductsAsync();
    Task<GetProductResponse?> GetProductByIdAsync(int id);
    Task<GetProductResponse> CreateProductAsync(CreateProductRequest product);
}
