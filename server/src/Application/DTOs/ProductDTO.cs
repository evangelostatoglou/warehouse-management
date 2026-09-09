using System.ComponentModel.DataAnnotations;

namespace WM.Application.DTOs;

public class GetProductResponse
{
    public int Id { get; init; }

    public String Sku { get; init; } = String.Empty;

    public String Name { get; init; } = String.Empty;

    public decimal Price { get; init; }

    public bool IsActive { get; init; }
}

public class CreateProductRequest
{
    [Required]
    [StringLength(50)]
    public String Sku {get; init;} = String.Empty;

    [Required]
    [StringLength(200)]
    public String Name {get; init;} = String.Empty;

    [StringLength(1000)]
    public String? Description {get; init;} = String.Empty;

    [Required]
    [Range(0.01, 999999)]
    public decimal Price {get; init;}

}

public class UpdateProductRequest
{
    [Required]
    [StringLength(50)]
    public String Sku {get; init;} = String.Empty;

    [Required]
    [StringLength(200)]
    public String Name {get; init;} = String.Empty;

    [StringLength(1000)]
    public String? Description {get; init;} = String.Empty;

    [Required]
    [Range(0.01, 999999)]
    public decimal Price {get; init;}
}


/// <summary>
/// bool? IsActive
/// </summary>
public class UpdateProductStatusRequest
{
    [Required]
    public bool? IsActive { get; init; }
}



