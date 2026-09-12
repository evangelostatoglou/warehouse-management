using WM.Domain.Entities;

namespace WM.Application.Services;

public interface IInventoryRepository
{
    Task<IReadOnlyList<Inventory>> GetAllInventoryAsync(int? warehouseId, int? productId);
    Task<IReadOnlyList<Inventory>> GetLowStockInventoryAsync();
    Task<Inventory?> GetInventoryByWarehouseAndProductAsync(int warehouseId, int productId);
    Task<bool> ProductExistsAsync(int productId);
    Task<bool> WarehouseExistsAsync(int warehouseId);
    Task CreateInventoryAsync(Inventory inventory);
    Task UpdateInventoryAsync(Inventory inventory);
    Task TransferInventoryAsync(Inventory sourceInventory, Inventory destinationInventory, bool destinationIsNew);
}
