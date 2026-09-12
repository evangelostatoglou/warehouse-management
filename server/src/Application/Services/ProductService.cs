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
                ReorderLevel = product.ReorderLevel,
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
            ReorderLevel = product.ReorderLevel,
            IsActive = product.IsActive
        };
    }


    public async Task<GetProductResponse> CreateProductAsync(CreateProductRequest product)
    {
        var p = new Product(product.Sku, product.Name, product.Description, product.Price, product.ReorderLevel);

        await _productRepository.CreateProductAsync(p);

        return new GetProductResponse
        {
            Id = p.Id,
            Sku = p.Sku,
            Name = p.Name,
            Price = p.Price,
            ReorderLevel = p.ReorderLevel,
            IsActive = p.IsActive
        };
    }


    public async Task<GetProductResponse?> UpdateProductAsync(UpdateProductRequest product, int id)
    {
        
        var p = await _productRepository.GetProductByIdAsync(id);
        if(p is null) return null;

        p.Update(product.Sku, product.Name, product.Description, product.Price, product.ReorderLevel);

        await _productRepository.UpdateProductAsync(p);

        return new GetProductResponse
        {
            Id = p.Id,
            Sku = p.Sku,
            Name = p.Name,
            Price = p.Price,
            ReorderLevel = p.ReorderLevel,
            IsActive = p.IsActive
        };
    }

    public async Task<GetProductResponse?> UpdateProductStatusAsync(UpdateProductStatusRequest status, int id)
    {
        if(status.IsActive is null) return null;
        var p = await _productRepository.GetProductByIdAsync(id);
        if(p is null) return null;

        if(status.IsActive == true) p.Activate();
        else p.Deactivate();

        await _productRepository.UpdateProductAsync(p);

        return new GetProductResponse
        {
            Id = p.Id,
            Sku = p.Sku,
            Name = p.Name,
            Price = p.Price,
            ReorderLevel = p.ReorderLevel,
            IsActive = p.IsActive
        };
    }















}
