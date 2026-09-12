using Microsoft.EntityFrameworkCore;
using WM.Application.Services;
using WM.Domain.Entities;

namespace WM.Infrastructure.Persistence;

public class InventoryRepository : IInventoryRepository
{
    private readonly AppDbContext _dbContext;

    public InventoryRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Inventory>> GetAllInventoryAsync(int? warehouseId, int? productId)
    {
        var inventory = _dbContext.Inventory.AsNoTracking();

        if(warehouseId is not null)
            inventory = inventory.Where(i => i.WarehouseId == warehouseId);

        if(productId is not null)
            inventory = inventory.Where(i => i.ProductId == productId);

        return await inventory
            .OrderBy(i => i.WarehouseId)
            .ThenBy(i => i.ProductId)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Inventory>> GetLowStockInventoryAsync()
    {
        return await (
            from inventory in _dbContext.Inventory.AsNoTracking()
            join product in _dbContext.Products.AsNoTracking()
                on inventory.ProductId equals product.Id
            where product.IsActive
                && inventory.QuantityOnHand - inventory.QuantityReserved < product.ReorderLevel
            orderby inventory.WarehouseId, inventory.ProductId
            select inventory
        ).ToListAsync();
    }

    public async Task<Inventory?> GetInventoryByWarehouseAndProductAsync(int warehouseId, int productId)
    {
        return await _dbContext.Inventory
            .AsNoTracking()
            .SingleOrDefaultAsync(i => i.WarehouseId == warehouseId && i.ProductId == productId);
    }

    public async Task<bool> ProductExistsAsync(int productId)
    {
        return await _dbContext.Products.AnyAsync(p => p.Id == productId);
    }

    public async Task<bool> WarehouseExistsAsync(int warehouseId)
    {
        return await _dbContext.Warehouses.AnyAsync(w => w.Id == warehouseId);
    }

    public async Task CreateInventoryAsync(Inventory inventory)
    {
        _dbContext.Inventory.Add(inventory);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateInventoryAsync(Inventory inventory)
    {
        _dbContext.Inventory.Update(inventory);
        await _dbContext.SaveChangesAsync();
    }

    public async Task TransferInventoryAsync(Inventory sourceInventory, Inventory destinationInventory, bool destinationIsNew)
    {
        _dbContext.Inventory.Update(sourceInventory);

        if(destinationIsNew)
            _dbContext.Inventory.Add(destinationInventory);
        else
            _dbContext.Inventory.Update(destinationInventory);

        await _dbContext.SaveChangesAsync();
    }
}
