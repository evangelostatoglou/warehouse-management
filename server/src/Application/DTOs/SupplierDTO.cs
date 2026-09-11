using System.ComponentModel.DataAnnotations;

namespace WM.Application.DTOs;

public class GetSupplierResponse
{
    public int Id {get;set;}
    public String Name {get;set;} = String.Empty;
    public String Email {get;set;} = String.Empty;
    public bool IsActive {get;set;}
}

public class CreateSupplierRequest
{
    [Required]
    [StringLength(150)]
    public String Name {get; init;} = String.Empty;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public String Email {get; init;} = String.Empty;
}

public class UpdateSupplierRequest
{
    [Required]
    [StringLength(150)]
    public String Name {get; init;} = String.Empty;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public String Email {get; init;} = String.Empty;
}

public class UpdateSupplierStatusRequest
{
    [Required]
    public bool? IsActive {get; init;}
}
