using Microsoft.AspNetCore.Mvc;
using WM.Application.DTOs;
using WM.Application.Services;

namespace WM.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GetCustomerResponse>>> GetAllCustomersAsync()
    {
        var customers = await _customerService.GetAllCustomersAsync();
        return Ok(customers);
    }

    [HttpGet("{id:int}", Name = "GetCustomerByIdRoute")]
    public async Task<ActionResult<GetCustomerResponse>> GetCustomerByIdAsync(int id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if(customer is null) return NotFound();
        else return Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult<GetCustomerResponse>> CreateCustomerAsync([FromBody] CreateCustomerRequest customer)
    {
        var c = await _customerService.CreateCustomerAsync(customer);
        return CreatedAtRoute("GetCustomerByIdRoute", new {id=c.Id}, c);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<GetCustomerResponse>> UpdateCustomerAsync(int id, [FromBody] UpdateCustomerRequest customer)
    {
        var c = await _customerService.UpdateCustomerAsync(customer, id);
        if(c is null) return NotFound();
        else return Ok(c);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult<GetCustomerResponse>> UpdateCustomerStatusAsync(int id, [FromBody] UpdateCustomerStatusRequest status)
    {
        var c = await _customerService.UpdateCustomerStatusAsync(status, id);
        if(c is null) return NotFound();
        else return Ok(c);
    }
}
