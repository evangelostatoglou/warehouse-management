namespace WM.Domain.Entities;

public class User
{
    public int Id { get; private set; }
    public String Name { get; private set; } = String.Empty;
    public String Email { get; private set; } = String.Empty;
    public String PasswordHash { get; private set; } = String.Empty;
    public char Role { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private User() { }

    public User(String name, String email, String passwordHash, char role)
    {
        if (String.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        if (String.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));

        email = email.Trim().ToLowerInvariant();

        if (!email.Contains('@') || email.Length > 255)
            throw new ArgumentException("Email is invalid.", nameof(email));

        if (String.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash is required.", nameof(passwordHash));

        if (passwordHash.Length > 500)
            throw new ArgumentException("Password hash is too long.", nameof(passwordHash));

        role = char.ToUpperInvariant(role);

        if (role is not ('A' or 'W' or 'S' or 'V'))
            throw new ArgumentException("Role must be A, W, S, or V.", nameof(role));

        Name = name.Trim();
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }
}
