using System.ComponentModel.DataAnnotations;

namespace WM.Application.DTOs;

public class GetCustomerResponse
{
    public int Id {get;set;}
    public String Name {get;set;} = String.Empty;
    public String Email {get;set;} = String.Empty;
    public String? Phone {get;set;}
    public bool IsActive {get;set;}
}

public class CreateCustomerRequest
{
    [Required]
    [StringLength(150)]
    public String Name {get; init;} = String.Empty;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public String Email {get; init;} = String.Empty;

    [StringLength(50)]
    public String? Phone {get; init;}
}

public class UpdateCustomerRequest
{
    [Required]
    [StringLength(150)]
    public String Name {get; init;} = String.Empty;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public String Email {get; init;} = String.Empty;

    [StringLength(50)]
    public String? Phone {get; init;}
}

public class UpdateCustomerStatusRequest
{
    [Required]
    public bool? IsActive {get; init;}
}
