using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Application.Features.Products.DTOs;
using WarehouseManagement.Application.Features.Products.Services;

namespace WarehouseManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductResponse>>> GetAll()
    {
        var products = await _productService.GetAllAsync();

        return Ok(products);
    }
}