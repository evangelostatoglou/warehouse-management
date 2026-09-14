
namespace WM.Application.Services;


public interface IPasswordService
{
    String HashPassword(String Password);
    bool VerifyPassword(String Password, String passwodHash);
}