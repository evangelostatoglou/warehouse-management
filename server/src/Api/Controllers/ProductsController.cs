using Microsoft.AspNetCore.Mvc;
using WM.Application.DTOs;
using WM.Application.Services;

namespace WM.Api.Controllers;

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
    public async Task<ActionResult<IReadOnlyList<GetProductResponse>>> GetAllProductsAsync()
    {
        var products = await _productService.GetAllProductsAsync();

        return Ok(products);
    }


    [HttpGet("{id:int}", Name = "GetProductByIdRoute")]
    public async Task<ActionResult<GetProductResponse>> GetProductByIdAsync(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);

        if(product is null) return NotFound();
        else return Ok(product);
    }



    [HttpPost]
    public async Task<ActionResult<GetProductResponse>> CreateProductAsync([FromBody] CreateProductRequest product)
    {
        var p = await _productService.CreateProductAsync(product);

        return CreatedAtRoute("GetProductByIdRoute", new {id=p.Id}, p);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<GetProductResponse>> UpdateProductAsync(int id, [FromBody] UpdateProductRequest product)
    {
        var p = await _productService.UpdateProductAsync(product, id);
        if(p is null) return NotFound();
        else return Ok(p);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult<GetProductResponse>> UpdateProductStatusAsync(int id, [FromBody] UpdateProductStatusRequest status)
    {
        var p = await _productService.UpdateProductStatusAsync(status, id);
        if(p is null) return NotFound();
        else return Ok(p);
    }















}
