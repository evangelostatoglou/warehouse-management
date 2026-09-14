using WM.Application.DTOs;
using WM.Domain.Entities;

namespace WM.Application.Services;

public interface ITokenService
{
    LoginResponse CreateToken(User user);
}