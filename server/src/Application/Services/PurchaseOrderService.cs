

using WM.Application.DTOs;
using WM.Domain.Entities;

namespace WM.Application.Services;

public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly ISupplierRepository _supplierRepository;
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;

    public PurchaseOrderService(
        IPurchaseOrderRepository purchaseOrderRepository,
        ISupplierRepository supplierRepository,
        IWarehouseRepository warehouseRepository,
        IUserRepository userRepository,
        IProductRepository productRepository)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _supplierRepository = supplierRepository;
        _warehouseRepository = warehouseRepository;
        _userRepository = userRepository;
        _productRepository = productRepository;
    }

    public async Task<IReadOnlyList<GetPurchaseOrderResponse>> GetAllPurchaseOrdersAsync()
    {
        var purchaseOrders = await _purchaseOrderRepository.GetAllPurchaseOrdersAsync();

        return purchaseOrders.Select(purchaseOrder => new GetPurchaseOrderResponse
        {
            Id = purchaseOrder.Id,
            SupplierId = purchaseOrder.SupplierId,
            WarehouseId = purchaseOrder.WarehouseId,
            CreatedBy = purchaseOrder.CreatedBy,
            Status = purchaseOrder.Status,
            CreatedAt = purchaseOrder.CreatedAt,
            Items = purchaseOrder.Items.Select(item => new GetPurchaseOrderItemResponse
            {
                Id = item.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitCost = item.UnitCost,
                TotalCost = item.Quantity * item.UnitCost
            }).ToList()
        }).ToList();
    }

    public async Task<GetPurchaseOrderResponse?> GetPurchaseOrderByIdAsync(int id)
    {
        var purchaseOrder = await _purchaseOrderRepository.GetPurchaseOrderByIdAsync(id);
        if(purchaseOrder is null) return null;

        return new GetPurchaseOrderResponse
        {
            Id = purchaseOrder.Id,
            SupplierId = purchaseOrder.SupplierId,
            WarehouseId = purchaseOrder.WarehouseId,
            CreatedBy = purchaseOrder.CreatedBy,
            Status = purchaseOrder.Status,
            CreatedAt = purchaseOrder.CreatedAt,
            Items = purchaseOrder.Items.Select(item => new GetPurchaseOrderItemResponse
            {
                Id = item.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitCost = item.UnitCost,
                TotalCost = item.Quantity * item.UnitCost
            }).ToList()
        };
    }

    public async Task<GetPurchaseOrderResponse?> CreatePurchaseOrderAsync(CreatePurchaseOrderRequest purchaseOrder, int createdBy)
    {
        var supplier = await _supplierRepository.GetSupplierByIdAsync(purchaseOrder.SupplierId);
        var warehouse = await _warehouseRepository.GetWarehouseByIdAsync(purchaseOrder.WarehouseId);
        var user = await _userRepository.GetUserByIdAsync(createdBy);

        if(supplier is null || warehouse is null || user is null) return null;

        if(!supplier.IsActive || !warehouse.IsActive || !user.IsActive)
            throw new ArgumentException("Supplier, warehouse, and user must be active.");

        if(purchaseOrder.Items.GroupBy(item => item.ProductId).Any(group => group.Count() > 1))
            throw new ArgumentException("Each product can appear only once in a purchase order.");

        var newPurchaseOrder = new PurchaseOrder(
            purchaseOrder.SupplierId,
            purchaseOrder.WarehouseId,
            createdBy);

        foreach(var item in purchaseOrder.Items)
        {
            var product = await _productRepository.GetProductByIdAsync(item.ProductId);

            if(product is null) return null;

            if(!product.IsActive)
                throw new ArgumentException("All products in a purchase order must be active.");

            newPurchaseOrder.AddItem(item.ProductId, item.Quantity, item.UnitCost);
        }

        await _purchaseOrderRepository.CreatePurchaseOrderAsync(newPurchaseOrder);

        return new GetPurchaseOrderResponse
        {
            Id = newPurchaseOrder.Id,
            SupplierId = newPurchaseOrder.SupplierId,
            WarehouseId = newPurchaseOrder.WarehouseId,
            CreatedBy = newPurchaseOrder.CreatedBy,
            Status = newPurchaseOrder.Status,
            CreatedAt = newPurchaseOrder.CreatedAt,
            Items = newPurchaseOrder.Items.Select(item => new GetPurchaseOrderItemResponse
            {
                Id = item.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitCost = item.UnitCost,
                TotalCost = item.Quantity * item.UnitCost
            }).ToList()
        };
    }

    public async Task<GetPurchaseOrderResponse?> ReceivePurchaseOrderAsync(int id)
    {
        var purchaseOrder = await _purchaseOrderRepository.ReceivePurchaseOrderAsync(id);
        if(purchaseOrder is null) return null;

        return new GetPurchaseOrderResponse
        {
            Id = purchaseOrder.Id,
            SupplierId = purchaseOrder.SupplierId,
            WarehouseId = purchaseOrder.WarehouseId,
            CreatedBy = purchaseOrder.CreatedBy,
            Status = purchaseOrder.Status,
            CreatedAt = purchaseOrder.CreatedAt,
            Items = purchaseOrder.Items.Select(item => new GetPurchaseOrderItemResponse
            {
                Id = item.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitCost = item.UnitCost,
                TotalCost = item.Quantity * item.UnitCost
            }).ToList()
        };
    }
}
