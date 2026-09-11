namespace WM.Domain.Entities;

public class Customer
{
    public int Id { get; private set; }
    public String Name { get; private set; } = String.Empty;
    public String Email { get; private set; } = String.Empty;
    public String? Phone { get; private set; }
    public bool IsActive { get; private set; }

    private Customer() { }

    public Customer(String name, String email, String? phone)
    {
        if (String.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        if (String.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));

        email = email.Trim().ToLowerInvariant();

        if (!email.Contains('@') || email.Length > 255)
            throw new ArgumentException("Email is invalid.", nameof(email));

        Name = name.Trim();
        Email = email;
        Phone = phone?.Trim();
        IsActive = true;
    }

    public void Update(String name, String email, String? phone)
    {
        if (String.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        if (String.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));

        email = email.Trim().ToLowerInvariant();

        if (!email.Contains('@') || email.Length > 255)
            throw new ArgumentException("Email is invalid.", nameof(email));

        Name = name.Trim();
        Email = email;
        Phone = phone?.Trim();
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
