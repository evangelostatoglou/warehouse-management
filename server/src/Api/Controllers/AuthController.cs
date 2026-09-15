using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WM.Application.DTOs;
using WM.Application.Services;

namespace WM.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public AuthController(
        IAuthService authService,
        IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> LoginAsync([FromBody] LoginRequest login)
    {
        var response = await _authService.LoginAsync(login);

        if(response is null)
            return Unauthorized();
        
        Response.Cookies.Append("access_token", response.Token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // because it is localhost
                SameSite = SameSiteMode.Lax,
                Expires = new DateTimeOffset(response.ExpiresAt),
                Path = "/"
            });

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<GetUserResponse>> RegisterAsync(
        [FromBody] RegisterRequest register)
    {
        bool usersExist = await _authService.HasUsersAsync();

        if(usersExist)
            return Forbid();

        var user = await _authService.RegisterAsync(register);

        return CreatedAtRoute(
            "GetUserByIdRoute",
            new { id = user.Id },
            user);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<GetUserResponse>> GetCurrentUserAsync()
    {
        var userIdValue = User
            .FindFirst(ClaimTypes.NameIdentifier)?
            .Value;

        if(!int.TryParse(userIdValue, out int userId))
            return Unauthorized();

        var user = await _userService.GetUserByIdAsync(userId);

        if(user is null || !user.IsActive)
            return Unauthorized();

        return Ok(user);
    }
}
