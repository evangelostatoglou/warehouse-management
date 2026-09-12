using System.ComponentModel.DataAnnotations;

namespace WM.Application.DTOs;

public class GetInventoryResponse
{
    public int Id {get;set;}
    public int ProductId {get;set;}
    public int WarehouseId {get;set;}
    public int QuantityOnHand {get;set;}
    public int QuantityReserved {get;set;}
    public int AvailableQuantity {get;set;}
}

public class InventoryAdjustmentRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int ProductId {get; init;}

    [Required]
    [Range(1, int.MaxValue)]
    public int WarehouseId {get; init;}

    [Range(-int.MaxValue, int.MaxValue)]
    public int QuantityChange {get; init;}
}

public class InventoryTransferRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int ProductId {get; init;}

    [Required]
    [Range(1, int.MaxValue)]
    public int SourceWarehouseId {get; init;}

    [Required]
    [Range(1, int.MaxValue)]
    public int DestinationWarehouseId {get; init;}

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity {get; init;}
}

public class TransferInventoryResponse
{
    public GetInventoryResponse SourceInventory {get;set;} = new GetInventoryResponse();
    public GetInventoryResponse DestinationInventory {get;set;} = new GetInventoryResponse();
}
