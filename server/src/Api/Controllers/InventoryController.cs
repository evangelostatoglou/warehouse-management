using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WM.Application.DTOs;
using WM.Application.Services;

namespace WM.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GetInventoryResponse>>> GetAllInventoryAsync([FromQuery] int? warehouseId, [FromQuery] int? productId)
    {
        var inventory = await _inventoryService.GetAllInventoryAsync(warehouseId, productId);
        return Ok(inventory);
    }

    [HttpGet("low-stock")]
    public async Task<ActionResult<IReadOnlyList<GetInventoryResponse>>> GetLowStockInventoryAsync()
    {
        var inventory = await _inventoryService.GetLowStockInventoryAsync();
        return Ok(inventory);
    }

    [HttpGet("{warehouseId:int}/{productId:int}")]
    public async Task<ActionResult<GetInventoryResponse>> GetInventoryByWarehouseAndProductAsync( int warehouseId, int productId)
    {
        var inventory = await _inventoryService.GetInventoryByWarehouseAndProductAsync(warehouseId, productId);
        if(inventory is null) return NotFound();
        else return Ok(inventory);
    }

    [Authorize(Roles = "A,W")]
    [HttpPost("adjust")]
    public async Task<ActionResult<GetInventoryResponse>> AdjustInventoryAsync([FromBody] InventoryAdjustmentRequest adjustment)
    {
        var inventory = await _inventoryService.AdjustInventoryAsync(adjustment);
        if(inventory is null) return NotFound();
        else return Ok(inventory);
    }

    [Authorize(Roles = "A,W")]
    [HttpPost("transfer")]
    public async Task<ActionResult<TransferInventoryResponse>> TransferInventoryAsync([FromBody] InventoryTransferRequest transfer)
    {
        var inventory = await _inventoryService.TransferInventoryAsync(transfer);
        if(inventory is null) return NotFound();
        else return Ok(inventory);
    }
}
