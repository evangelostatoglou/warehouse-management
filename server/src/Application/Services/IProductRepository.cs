using WM.Domain.Entities;

namespace WM.Application.Services;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task CreateProductAsync(Product product);
}