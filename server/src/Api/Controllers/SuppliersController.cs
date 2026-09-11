using Microsoft.AspNetCore.Mvc;
using WM.Application.DTOs;
using WM.Application.Services;

namespace WM.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SuppliersController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GetSupplierResponse>>> GetAllSuppliersAsync()
    {
        var suppliers = await _supplierService.GetAllSuppliersAsync();
        return Ok(suppliers);
    }

    [HttpGet("{id:int}", Name = "GetSupplierByIdRoute")]
    public async Task<ActionResult<GetSupplierResponse>> GetSupplierByIdAsync(int id)
    {
        var supplier = await _supplierService.GetSupplierByIdAsync(id);
        if(supplier is null) return NotFound();
        else return Ok(supplier);
    }

    [HttpPost]
    public async Task<ActionResult<GetSupplierResponse>> CreateSupplierAsync([FromBody] CreateSupplierRequest supplier)
    {
        var s = await _supplierService.CreateSupplierAsync(supplier);
        return CreatedAtRoute("GetSupplierByIdRoute", new {id=s.Id}, s);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<GetSupplierResponse>> UpdateSupplierAsync(int id, [FromBody] UpdateSupplierRequest supplier)
    {
        var s = await _supplierService.UpdateSupplierAsync(supplier, id);
        if(s is null) return NotFound();
        else return Ok(s);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult<GetSupplierResponse>> UpdateSupplierStatusAsync(int id, [FromBody] UpdateSupplierStatusRequest status)
    {
        var s = await _supplierService.UpdateSupplierStatusAsync(status, id);
        if(s is null) return NotFound();
        else return Ok(s);
    }
}
