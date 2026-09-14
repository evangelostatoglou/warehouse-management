

namespace WM.Application.Services;


public class PasswordService: IPasswordService
{
    public String HashPassword(String Password)
    {
        return BCrypt.Net.BCrypt.HashPassword(Password);
    }

    public bool VerifyPassword(String password, String passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}

