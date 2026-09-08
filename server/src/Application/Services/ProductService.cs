using WM.Application.DTOs;
using WM.Domain.Entities;


namespace WM.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IReadOnlyList<GetProductResponse>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllProductsAsync();

        return products
            .Select(product => new GetProductResponse
            {
                Id = product.Id,
                Sku = product.Sku,
                Name = product.Name,
                Price = product.Price,
                IsActive = product.IsActive
            })
            .ToList();
    }

    public async Task<GetProductResponse?> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        if(product is null) return null;

        return new GetProductResponse
        {
            Id = product.Id,
            Sku = product.Sku,
            Name = product.Name,
            Price = product.Price,
            IsActive = product.IsActive
        };
    }


    public async Task<GetProductResponse> CreateProductAsync(CreateProductRequest p)
    {
        var product = new Product(p.Sku, p.Name, p.Description, p.Price);

        await _productRepository.CreateProductAsync(product);

        return new GetProductResponse
        {
            Id = product.Id,
            Sku = product.Sku,
            Name = product.Name,
            Price = product.Price,
            IsActive = product.IsActive
        };
    }















}