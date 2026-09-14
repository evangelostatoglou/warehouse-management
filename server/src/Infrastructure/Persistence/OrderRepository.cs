
using Microsoft.EntityFrameworkCore;
using WM.Application.Services;
using WM.Domain.Entities;

namespace WM.Infrastructure.Persistence;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _dbContext;

    public OrderRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Order>> GetAllOrdersAsync()
    {
        return await _dbContext.Orders
            .AsNoTracking()
            .Include(o => o.Items)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await _dbContext.Orders
            .AsNoTracking()
            .Include(o => o.Items)
            .SingleOrDefaultAsync(o => o.Id == id);
    }

    public async Task CreateOrderAsync(Order order)
    {
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Order?> ConfirmOrderAsync(int id)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        var order = await GetOrderForActionAsync(id);
        if(order is null) return null;

        order.Confirm();

        foreach(var item in order.Items.OrderBy(item => item.ProductId)) // we sort by to reduce the risk of 2 simultaneous orders lock
        {
            var inventory = await GetLockedInventoryAsync(order.WarehouseId, item.ProductId);
            inventory.Reserve(item.Quantity);
        }

        await _dbContext.SaveChangesAsync();
        await transaction.CommitAsync();

        return order;
    }

    public async Task<Order?> CancelOrderAsync(int id)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        var order = await GetOrderForActionAsync(id);
        if(order is null) return null;

        bool releaseReservations = order.Status == "CONFIRMED";

        order.Cancel();

        if(releaseReservations)
        {
            foreach(var item in order.Items.OrderBy(item => item.ProductId))
            {
                var inventory = await GetLockedInventoryAsync(order.WarehouseId, item.ProductId);
                inventory.ReleaseReservation(item.Quantity);
            }
        }

        await _dbContext.SaveChangesAsync();
        await transaction.CommitAsync();

        return order;
    }

    public async Task<Order?> ShipOrderAsync(int id) 
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        var order = await GetOrderForActionAsync(id);
        if(order is null) return null;

        order.Ship();

        foreach(var item in order.Items.OrderBy(item => item.ProductId))
        {
            var inventory = await GetLockedInventoryAsync(order.WarehouseId, item.ProductId);
            inventory.Ship(item.Quantity);
        }

        await _dbContext.SaveChangesAsync();
        await transaction.CommitAsync();

        return order;
    }

    private async Task<Order?> GetOrderForActionAsync(int id)
    {
        var orderRows = await _dbContext.Orders
            .FromSqlInterpolated($"SELECT * FROM \"Orders\" WHERE \"Id\" = {id} FOR UPDATE")
            .ToListAsync();

        var order = orderRows.SingleOrDefault();
        if(order is null) return null;

        await _dbContext.Entry(order)
            .Collection(o => o.Items)
            .LoadAsync();

        return order;
    }

    private async Task<Inventory> GetLockedInventoryAsync(int warehouseId, int productId)
    {
        var inventoryRows = await _dbContext.Inventory
            .FromSqlInterpolated($"SELECT * FROM \"Inventory\" WHERE \"WarehouseId\" = {warehouseId} AND \"ProductId\" = {productId} FOR UPDATE")
            .ToListAsync();

        var inventory = inventoryRows.SingleOrDefault();

        if(inventory is null)
            throw new InvalidOperationException("No inventory exists for this product at the order warehouse.");

        return inventory;
    }
}
