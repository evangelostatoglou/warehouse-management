using Microsoft.EntityFrameworkCore;
using Npgsql;
using WM.Application.Services;
using WM.Domain.Entities;



namespace WM.Infrastructure.Persistence;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _dbContext;

    public ProductRepository(
        AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Product>> GetAllProductsAsync()
    {
        return await _dbContext.Products
            .AsNoTracking() // we only read, no need to track for later update/delete
            .OrderBy(product => product.Name)
            .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _dbContext.Products.AsNoTracking().SingleOrDefaultAsync(p => p.Id == id);
    }


    public async Task CreateProductAsync(Product product)
    {
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
    }


    public async Task UpdateProductAsync(Product product)
    {
        _dbContext.Products.Update(product);
        await _dbContext.SaveChangesAsync();
    }

    







}
