namespace WM.Domain.Entities;

public class Supplier
{
    public int Id { get; private set; }
    public String Name { get; private set; } = String.Empty;
    public String Email { get; private set; } = String.Empty;
    public bool IsActive { get; private set; }

    private Supplier() { }

    public Supplier(String name, String email)
    {
        if (String.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Supplier name is required.", nameof(name));

        if (String.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Supplier email is required.", nameof(email));

        email = email.Trim().ToLowerInvariant();

        if (!email.Contains('@') || email.Length > 255)
            throw new ArgumentException("Supplier email is invalid.", nameof(email));

        Name = name.Trim();
        Email = email;
        IsActive = true;
    }
}
