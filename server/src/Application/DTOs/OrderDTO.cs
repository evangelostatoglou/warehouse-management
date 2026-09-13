using System.ComponentModel.DataAnnotations;

namespace WM.Application.DTOs;

public class GetOrderResponse
{
    public int Id {get;set;}
    public int CustomerId {get;set;}
    public int WarehouseId {get;set;}
    public int CreatedBy {get;set;}
    public String Status {get;set;} = String.Empty;
    public DateTime CreatedAt {get;set;}
    public decimal TotalAmount {get;set;}
    public List<GetOrderItemResponse> Items {get;set;} = [];
}

public class GetOrderItemResponse
{
    public int Id {get;set;}
    public int ProductId {get;set;}
    public int Quantity {get;set;}
    public decimal UnitPrice {get;set;}
    public decimal TotalPrice {get;set;}
}

public class CreateOrderRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int CustomerId {get;init;}

    [Required]
    [Range(1, int.MaxValue)]
    public int WarehouseId {get;init;}

    [Required]
    [Range(1, int.MaxValue)]
    public int CreatedBy {get;init;}

    [MinLength(1)]
    public List<CreateOrderItemRequest> Items {get;init;} = [];
}

public class CreateOrderItemRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int ProductId {get;init;}

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity {get;init;}
}
