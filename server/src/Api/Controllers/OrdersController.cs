
using Microsoft.AspNetCore.Mvc;
using WM.Application.DTOs;
using WM.Application.Services;


namespace WM.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController: ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GetOrderResponse>>> GetAllOrdersAsync()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(orders);
    }

    [HttpGet("{id:int}", Name = "GetOrderByIdRoute")]
    public async Task<ActionResult<GetOrderResponse>> GetOrderByIdAsync(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if(order is null) return NotFound();
        else return Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<GetOrderResponse>> CreateOrderAsync([FromBody] CreateOrderRequest order)
    {
        var newOrder = await _orderService.CreateOrderAsync(order);
        if(newOrder is null) return NotFound();
        else return CreatedAtRoute("GetOrderByIdRoute", new {id=newOrder.Id}, newOrder);
    }
 
    [HttpPost("{id:int}/confirm")]
    public async Task<ActionResult<GetOrderResponse>> ConfirmOrderAsync(int id)
    {
        var order = await _orderService.ConfirmOrderAsync(id);
        if(order is null) return NotFound();
        else return Ok(order);
    }

    [HttpPost("{id:int}/cancel")]
    public async Task<ActionResult<GetOrderResponse>> CancelOrderAsync(int id)
    {
        var order = await _orderService.CancelOrderAsync(id);
        if(order is null) return NotFound();
        else return Ok(order);
    }

    [HttpPost("{id:int}/ship")]
    public async Task<ActionResult<GetOrderResponse>> ShipOrderAsync(int id)
    {
        var order = await _orderService.ShipOrderAsync(id);
        if(order is null) return NotFound();
        else return Ok(order);
    }
}
