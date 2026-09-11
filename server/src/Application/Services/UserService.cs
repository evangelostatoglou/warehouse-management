using WM.Application.DTOs;
using WM.Domain.Entities;

namespace WM.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IReadOnlyList<GetUserResponse>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();

        return users.Select(user => new GetUserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        }).ToList();
    }

    public async Task<GetUserResponse?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if(user is null) return null;

        return new GetUserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        };
    }

    public async Task<GetUserResponse> CreateUserAsync(CreateUserRequest user)
    {
        var u = new User(user.Name, user.Email, user.PasswordHash, user.Role);

        await _userRepository.CreateUserAsync(u);

        return new GetUserResponse
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            Role = u.Role,
            IsActive = u.IsActive,
            CreatedAt = u.CreatedAt
        };
    }

    public async Task<GetUserResponse?> UpdateUserAsync(UpdateUserRequest user, int id)
    {
        var u = await _userRepository.GetUserByIdAsync(id);
        if(u is null) return null;

        u.Update(user.Name, user.Email, user.Role);

        await _userRepository.UpdateUserAsync(u);

        return new GetUserResponse
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            Role = u.Role,
            IsActive = u.IsActive,
            CreatedAt = u.CreatedAt
        };
    }

    public async Task<GetUserResponse?> UpdateUserStatusAsync(UpdateUserStatusRequest status, int id)
    {
        if(status.IsActive is null) return null;

        var u = await _userRepository.GetUserByIdAsync(id);
        if(u is null) return null;

        if(status.IsActive == true) u.Activate();
        else u.Deactivate();

        await _userRepository.UpdateUserAsync(u);

        return new GetUserResponse
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            Role = u.Role,
            IsActive = u.IsActive,
            CreatedAt = u.CreatedAt
        };
    }
}
