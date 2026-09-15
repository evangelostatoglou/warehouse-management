

using System.ComponentModel.DataAnnotations;

namespace WM.Application.DTOs;

public class RegisterRequest
{
    [Required]
    [MaxLength(150)]
    public String Name {get;init;} = String.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public String Email {get;init;} = String.Empty;

    [Required]
    [StringLength(128, MinimumLength = 8)]
    public String Password {get;init;} = String.Empty;

}

public class LoginRequest
{
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public String Email {get;init;} = String.Empty;

    [Required]
    [StringLength(128, MinimumLength = 8)]
    public String Password {get;init;} = String.Empty;
}

public class LoginResponse
{
    public String Token {get;init;} = String.Empty;

    public DateTime ExpiresAt {get;init;}

    public GetUserResponse User {get;init;} = new();
}
