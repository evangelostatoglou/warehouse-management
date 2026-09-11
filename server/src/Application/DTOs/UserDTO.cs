using System.ComponentModel.DataAnnotations;

namespace WM.Application.DTOs;

public class GetUserResponse
{
    public int Id {get;set;}
    public String Name {get;set;} = String.Empty;
    public String Email {get;set;} = String.Empty;
    public char Role {get;set;}
    public bool IsActive {get;set;}
    public DateTime CreatedAt {get;set;}
}

public class CreateUserRequest
{
    [Required]
    [StringLength(150)]
    public String Name {get; init;} = String.Empty;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public String Email {get; init;} = String.Empty;

    [Required]
    [StringLength(500)]
    public String PasswordHash {get; init;} = String.Empty;

    public char Role {get; init;}
}

public class UpdateUserRequest
{
    [Required]
    [StringLength(150)]
    public String Name {get; init;} = String.Empty;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public String Email {get; init;} = String.Empty;

    public char Role {get; init;}
}

public class UpdateUserStatusRequest
{
    [Required]
    public bool? IsActive {get; init;}
}
