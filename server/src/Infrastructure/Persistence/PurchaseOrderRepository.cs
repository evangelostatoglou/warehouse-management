
using Microsoft.EntityFrameworkCore;
using WM.Application.Services;
using WM.Domain.Entities;

namespace WM.Infrastructure.Persistence;

public class PurchaseOrderRepository : IPurchaseOrderRepository
{
    private readonly AppDbContext _dbContext;

    public PurchaseOrderRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<PurchaseOrder>> GetAllPurchaseOrdersAsync()
    {
        return await _dbContext.PurchaseOrders
            .AsNoTracking()
            .Include(purchaseOrder => purchaseOrder.Items)
            .OrderByDescending(purchaseOrder => purchaseOrder.CreatedAt)
            .ToListAsync();
    }

    public async Task<PurchaseOrder?> GetPurchaseOrderByIdAsync(int id)
    {
        return await _dbContext.PurchaseOrders
            .AsNoTracking()
            .Include(purchaseOrder => purchaseOrder.Items)
            .SingleOrDefaultAsync(purchaseOrder => purchaseOrder.Id == id);
    }

    public async Task CreatePurchaseOrderAsync(PurchaseOrder purchaseOrder)
    {
        _dbContext.PurchaseOrders.Add(purchaseOrder);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<PurchaseOrder?> ReceivePurchaseOrderAsync(int id)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        var purchaseOrder = await GetPurchaseOrderForActionAsync(id);
        if(purchaseOrder is null) return null;

        purchaseOrder.Receive();

        foreach(var item in purchaseOrder.Items.OrderBy(item => item.ProductId))
        {
            var inventory = await GetLockedInventoryAsync(
                purchaseOrder.WarehouseId,
                item.ProductId);

            if(inventory is null)
            {
                inventory = new Inventory(
                    item.ProductId,
                    purchaseOrder.WarehouseId,
                    item.Quantity,
                    0);

                _dbContext.Inventory.Add(inventory);
            }
            else
            {
                inventory.Adjust(item.Quantity);
            }
        }

        await _dbContext.SaveChangesAsync();
        await transaction.CommitAsync();

        return purchaseOrder;
    }

    private async Task<PurchaseOrder?> GetPurchaseOrderForActionAsync(int id)
    {
        var purchaseOrderRows = await _dbContext.PurchaseOrders
            .FromSqlInterpolated($"SELECT * FROM \"PurchaseOrders\" WHERE \"Id\" = {id} FOR UPDATE")
            .ToListAsync();

        var purchaseOrder = purchaseOrderRows.SingleOrDefault();
        if(purchaseOrder is null) return null;

        await _dbContext.Entry(purchaseOrder)
            .Collection(purchaseOrder => purchaseOrder.Items)
            .LoadAsync();

        return purchaseOrder;
    }

    private async Task<Inventory?> GetLockedInventoryAsync(int warehouseId, int productId)
    {
        var inventoryRows = await _dbContext.Inventory
            .FromSqlInterpolated($"SELECT * FROM \"Inventory\" WHERE \"WarehouseId\" = {warehouseId} AND \"ProductId\" = {productId} FOR UPDATE")
            .ToListAsync();

        return inventoryRows.SingleOrDefault();
    }
}

