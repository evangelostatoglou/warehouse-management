using Microsoft.AspNetCore.Mvc;
using WM.Application.DTOs;
using WM.Application.Services;

namespace WM.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehousesController: ControllerBase
{
    private readonly IWarehouseService _warehouseService;

    public WarehousesController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GetWarehouseResponse>>> GetAllWarehousesAsync()
    {
        var warehouses = await _warehouseService.GetAllWarehousesAsync();
        return Ok(warehouses);
    }

    [HttpGet("{id:int}", Name = "GetWarehouseByIdRoute")]
    public async Task<ActionResult<GetWarehouseResponse>> GetWarehouseByIdAsync(int id)
    {
        var warehouse = await _warehouseService.GetWarehouseByIdAsync(id);
        if(warehouse is null) return NotFound();
        else return Ok(warehouse);
    }

    [HttpPost]
    public async Task<ActionResult<GetWarehouseResponse>> CreateWarehouseAsync([FromBody] CreateWarehouseRequest warehouse)
    {
        var w = await _warehouseService.CreateWarehouseAsync(warehouse);

        return CreatedAtRoute("GetWarehouseByIdRoute", new {id=w.Id}, w);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<GetWarehouseResponse>> UpdateWarehouseAsync(int id, [FromBody] UpdateWarehouseRequest warehouse)
    {
        var w = await _warehouseService.UpdateWarehouseAsync(warehouse, id);
        if(w is null) return NotFound();
        else return Ok(w);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult<GetWarehouseResponse>> UpdateWarehouseStatusAsync(int id, [FromBody] UpdateWarehouseStatusRequest status)
    {
        var w = await _warehouseService.UpdateWarehouseStatusAsync(status, id);
        if(w is null) return NotFound();
        else return Ok(w);
    }

}


