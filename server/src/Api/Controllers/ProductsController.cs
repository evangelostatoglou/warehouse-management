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
    public async Task<ActionResult<GetProductResponse>> CreateProductAsync([FromBody] CreateProductRequest p)
    {
        var product = await _productService.CreateProductAsync(p);

        return CreatedAtRoute("GetProductByIdRoute", new {id=product.Id}, product);
    }

}
