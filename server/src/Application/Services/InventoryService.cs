using WM.Application.DTOs;
using WM.Domain.Entities;

namespace WM.Application.Services;

public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _inventoryRepository;

    public InventoryService(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public async Task<IReadOnlyList<GetInventoryResponse>> GetAllInventoryAsync(int? warehouseId, int? productId)
    {
        var inventory = await _inventoryRepository.GetAllInventoryAsync(warehouseId, productId);

        return inventory.Select(i => new GetInventoryResponse
        {
            Id = i.Id,
            ProductId = i.ProductId,
            WarehouseId = i.WarehouseId,
            QuantityOnHand = i.QuantityOnHand,
            QuantityReserved = i.QuantityReserved,
            AvailableQuantity = i.QuantityOnHand - i.QuantityReserved
        }).ToList();
    }

    public async Task<IReadOnlyList<GetInventoryResponse>> GetLowStockInventoryAsync()
    {
        var inventory = await _inventoryRepository.GetLowStockInventoryAsync();

        return inventory.Select(i => new GetInventoryResponse
        {
            Id = i.Id,
            ProductId = i.ProductId,
            WarehouseId = i.WarehouseId,
            QuantityOnHand = i.QuantityOnHand,
            QuantityReserved = i.QuantityReserved,
            AvailableQuantity = i.QuantityOnHand - i.QuantityReserved
        }).ToList();
    }

    public async Task<GetInventoryResponse?> GetInventoryByWarehouseAndProductAsync(int warehouseId, int productId)
    {
        var inventory = await _inventoryRepository.GetInventoryByWarehouseAndProductAsync(warehouseId, productId);
        if(inventory is null) return null;

        return new GetInventoryResponse
        {
            Id = inventory.Id,
            ProductId = inventory.ProductId,
            WarehouseId = inventory.WarehouseId,
            QuantityOnHand = inventory.QuantityOnHand,
            QuantityReserved = inventory.QuantityReserved,
            AvailableQuantity = inventory.QuantityOnHand - inventory.QuantityReserved
        };
    }

    public async Task<GetInventoryResponse?> AdjustInventoryAsync(InventoryAdjustmentRequest adjustment)
    {
        bool productExists = await _inventoryRepository.ProductExistsAsync(adjustment.ProductId);
        bool warehouseExists = await _inventoryRepository.WarehouseExistsAsync(adjustment.WarehouseId);

        if(!productExists || !warehouseExists) return null;

        var inventory = await _inventoryRepository.GetInventoryByWarehouseAndProductAsync(
            adjustment.WarehouseId,
            adjustment.ProductId);

        if(inventory is null)
        {
            if(adjustment.QuantityChange <= 0)
                throw new ArgumentException("A new inventory row must start with a positive quantity.", nameof(adjustment.QuantityChange));

            inventory = new Inventory(
                adjustment.ProductId,
                adjustment.WarehouseId,
                adjustment.QuantityChange,
                0);

            await _inventoryRepository.CreateInventoryAsync(inventory);
        }
        else
        {
            inventory.Adjust(adjustment.QuantityChange);
            await _inventoryRepository.UpdateInventoryAsync(inventory);
        }

        return new GetInventoryResponse
        {
            Id = inventory.Id,
            ProductId = inventory.ProductId,
            WarehouseId = inventory.WarehouseId,
            QuantityOnHand = inventory.QuantityOnHand,
            QuantityReserved = inventory.QuantityReserved,
            AvailableQuantity = inventory.QuantityOnHand - inventory.QuantityReserved
        };
    }

    public async Task<TransferInventoryResponse?> TransferInventoryAsync(InventoryTransferRequest transfer)
    {
        if(transfer.SourceWarehouseId == transfer.DestinationWarehouseId)
            throw new ArgumentException("Source and destination warehouses must be different.", nameof(transfer.DestinationWarehouseId));

        bool productExists = await _inventoryRepository.ProductExistsAsync(transfer.ProductId);
        bool sourceWarehouseExists = await _inventoryRepository.WarehouseExistsAsync(transfer.SourceWarehouseId);
        bool destinationWarehouseExists = await _inventoryRepository.WarehouseExistsAsync(transfer.DestinationWarehouseId);

        if(!productExists || !sourceWarehouseExists || !destinationWarehouseExists) return null;

        var sourceInventory = await _inventoryRepository.GetInventoryByWarehouseAndProductAsync(
            transfer.SourceWarehouseId,
            transfer.ProductId);

        if(sourceInventory is null)
            throw new ArgumentException("The source warehouse has no inventory for this product.", nameof(transfer.SourceWarehouseId));

        var destinationInventory = await _inventoryRepository.GetInventoryByWarehouseAndProductAsync(
            transfer.DestinationWarehouseId,
            transfer.ProductId);

        bool destinationIsNew = destinationInventory is null;

        if(destinationInventory is null)
        {
            destinationInventory = new Inventory(
                transfer.ProductId,
                transfer.DestinationWarehouseId,
                0,
                0);
        }

        sourceInventory.Adjust(-transfer.Quantity);
        destinationInventory.Adjust(transfer.Quantity);

        await _inventoryRepository.TransferInventoryAsync(
            sourceInventory,
            destinationInventory,
            destinationIsNew);

        return new TransferInventoryResponse
        {
            SourceInventory = new GetInventoryResponse
            {
                Id = sourceInventory.Id,
                ProductId = sourceInventory.ProductId,
                WarehouseId = sourceInventory.WarehouseId,
                QuantityOnHand = sourceInventory.QuantityOnHand,
                QuantityReserved = sourceInventory.QuantityReserved,
                AvailableQuantity = sourceInventory.QuantityOnHand - sourceInventory.QuantityReserved
            },
            DestinationInventory = new GetInventoryResponse
            {
                Id = destinationInventory.Id,
                ProductId = destinationInventory.ProductId,
                WarehouseId = destinationInventory.WarehouseId,
                QuantityOnHand = destinationInventory.QuantityOnHand,
                QuantityReserved = destinationInventory.QuantityReserved,
                AvailableQuantity = destinationInventory.QuantityOnHand - destinationInventory.QuantityReserved
            }
        };
    }
}
