
using System.ComponentModel.DataAnnotations;

namespace WM.Application.DTOs;

public class GetPurchaseOrderResponse
{
    public int Id { get; set; }
    public int SupplierId { get; init; }
    public int WarehouseId { get; init; }
    public int CreatedBy { get; init; }
    public String Status { get; init; } = String.Empty;
    public DateTime CreatedAt { get; init; }

    public List<GetPurchaseOrderItemResponse> Items { get; init; } = [];
}

public class GetPurchaseOrderItemResponse
{
    public int Id { get; init; }
    public int ProductId { get; init; }
    public int Quantity { get; init; }
    public decimal UnitCost { get; init; }
    public decimal TotalCost { get; init; }
}

public class CreatePurchaseOrderRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int SupplierId { get; init; }

    [Required]
    [Range(1, int.MaxValue)]
    public int WarehouseId { get; init; }

    [Required]
    [Range(1, int.MaxValue)]
    public int CreatedBy { get; init; }

    [MinLength(1)]
    public List<CreatePurchaseOrderItemRequest> Items { get; init; } = [];
}

public class CreatePurchaseOrderItemRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int ProductId { get; init; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; init; }

    [Required]
    [Range(typeof(decimal), "0.01", "999999999")]
    public decimal UnitCost { get; init; }
}
