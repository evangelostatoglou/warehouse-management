

using Microsoft.AspNetCore.Mvc;
using WM.Application.DTOs;
using WM.Application.Services;

namespace WM.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseOrdersController: ControllerBase
{
    private readonly IPurchaseOrderService _purchaseOrderService;

    public PurchaseOrdersController(IPurchaseOrderService purchaseOrderService)
    {
        _purchaseOrderService = purchaseOrderService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GetPurchaseOrderResponse>>> GetAllPurchaseOrdersAsync()
    {
        var purchaseOrders = await _purchaseOrderService.GetAllPurchaseOrdersAsync();
        return Ok(purchaseOrders);
    }

    [HttpGet("{id:int}", Name = "GetPurchaseOrderByIdRoute")]
    public async Task<ActionResult<GetPurchaseOrderResponse>> GetPurchaseOrderByIdAsync(int id)
    {
        var purchaseOrder = await _purchaseOrderService.GetPurchaseOrderByIdAsync(id);
        if (purchaseOrder is null) return NotFound();
        else return Ok(purchaseOrder);
    }

    [HttpPost]
    public async Task<ActionResult<GetPurchaseOrderResponse>> CreatePurchaseOrderAsync(
        [FromBody] CreatePurchaseOrderRequest purchaseOrder)
    {
        var newPurchaseOrder = await _purchaseOrderService.CreatePurchaseOrderAsync(purchaseOrder);
        if (newPurchaseOrder is null) return NotFound();
        else return CreatedAtRoute(
            "GetPurchaseOrderByIdRoute",
            new { id = newPurchaseOrder.Id },
            newPurchaseOrder);
    }

    [HttpPost("{id:int}/receive")]
    public async Task<ActionResult<GetPurchaseOrderResponse>> ReceivePurchaseOrderAsync(int id)
    {
        var purchaseOrder = await _purchaseOrderService.ReceivePurchaseOrderAsync(id);
        if (purchaseOrder is null) return NotFound();
        else return Ok(purchaseOrder);
    }
}


