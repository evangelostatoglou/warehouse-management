using Microsoft.AspNetCore.Mvc;
using WM.Application.DTOs;
using WM.Application.Services;

namespace WM.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GetUserResponse>>> GetAllUsersAsync()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id:int}", Name = "GetUserByIdRoute")]
    public async Task<ActionResult<GetUserResponse>> GetUserByIdAsync(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if(user is null) return NotFound();
        else return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<GetUserResponse>> CreateUserAsync([FromBody] CreateUserRequest user)
    {
        var u = await _userService.CreateUserAsync(user);
        return CreatedAtRoute("GetUserByIdRoute", new {id=u.Id}, u);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<GetUserResponse>> UpdateUserAsync(int id, [FromBody] UpdateUserRequest user)
    {
        var u = await _userService.UpdateUserAsync(user, id);
        if(u is null) return NotFound();
        else return Ok(u);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult<GetUserResponse>> UpdateUserStatusAsync(int id, [FromBody] UpdateUserStatusRequest status)
    {
        var u = await _userService.UpdateUserStatusAsync(status, id);
        if(u is null) return NotFound();
        else return Ok(u);
    }
}
