using WM.Application.DTOs;

namespace WM.Application.Services;

public interface IProductService
{
    /// <summary>
    /// Request all products in Prodects table
    /// </summary>
    /// <returns>Rows of products or empty list</returns>
    Task<IReadOnlyList<GetProductResponse>> GetAllProductsAsync();

    /// <summary>
    /// Looks for the product with the given ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns>null if doesnt exist else the Product Response</returns>
    Task<GetProductResponse?> GetProductByIdAsync(int id);

    /// <summary>
    /// Creates the product in the DB
    /// </summary>
    /// <param name="product"></param>
    /// <returns>the Prodect Response</returns>
    Task<GetProductResponse> CreateProductAsync(CreateProductRequest product);

    /// <summary>
    /// Handles the PUT request for a product
    /// </summary>
    /// <param name="product"></param>
    /// <param name="id"></param>
    /// <returns>ProductResponse</returns>
    Task<GetProductResponse?> UpdateProductAsync(UpdateProductRequest product, int id);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="status"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<GetProductResponse?> UpdateProductStatusAsync(UpdateProductStatusRequest status, int id);
}
