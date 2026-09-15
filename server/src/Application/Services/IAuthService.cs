using WM.Application.DTOs;

namespace WM.Application.Services;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest login);
    Task<GetUserResponse> RegisterAsync(RegisterRequest register);
    Task<bool> HasUsersAsync();
}
