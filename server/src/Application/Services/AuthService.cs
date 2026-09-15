using WM.Application.DTOs;
using WM.Domain.Entities;

namespace WM.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;

    public AuthService(
        IUserRepository userRepository,
        IPasswordService passwordService,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest login)
    {
        var user = await _userRepository.GetUserByEmailAsync(login.Email);

        if(user is null || !user.IsActive)
            return null;

        bool passwordIsCorrect = _passwordService.VerifyPassword(login.Password, user.PasswordHash);

        if(!passwordIsCorrect)
            return null;

        return _tokenService.CreateToken(user);
    }

    public async Task<GetUserResponse> RegisterAsync(RegisterRequest register)
    {
        var passwordHash = _passwordService.HashPassword(register.Password);

        var user = new User(
            register.Name,
            register.Email,
            passwordHash,
            'A');

        await _userRepository.CreateUserAsync(user);

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

    public async Task<bool> HasUsersAsync()
    {
        return await _userRepository.HasUsersAsync();
    }
}
