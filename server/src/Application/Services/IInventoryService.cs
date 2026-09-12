using WM.Application.DTOs;

namespace WM.Application.Services;

public interface IInventoryService
{
    Task<IReadOnlyList<GetInventoryResponse>> GetAllInventoryAsync(int? warehouseId, int? productId);
    Task<IReadOnlyList<GetInventoryResponse>> GetLowStockInventoryAsync();
    Task<GetInventoryResponse?> GetInventoryByWarehouseAndProductAsync(int warehouseId, int productId);
    Task<GetInventoryResponse?> AdjustInventoryAsync(InventoryAdjustmentRequest adjustment);
    Task<TransferInventoryResponse?> TransferInventoryAsync(InventoryTransferRequest transfer);
}
