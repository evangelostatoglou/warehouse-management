using WM.Application.DTOs;

namespace WM.Application.Services;

public interface IUserService
{
    Task<IReadOnlyList<GetUserResponse>> GetAllUsersAsync();
    Task<GetUserResponse?> GetUserByIdAsync(int id);
    Task<GetUserResponse> CreateUserAsync(CreateUserRequest user);
    Task<GetUserResponse?> UpdateUserAsync(UpdateUserRequest user, int id);
    Task<GetUserResponse?> UpdateUserStatusAsync(UpdateUserStatusRequest status, int id);
}
