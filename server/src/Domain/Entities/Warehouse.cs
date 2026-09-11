namespace WM.Domain.Entities;


public class Warehouse
{
    public int Id { get; private set; }

    public String Name { get; private set; } = String.Empty;

    public String Location { get; private set; } = String.Empty;

    public bool IsActive { get; private set; }

    private Warehouse(){}

    public Warehouse(String name, String location)
    {
        
        if (String.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));

        if (String.IsNullOrWhiteSpace(location))
            throw new ArgumentException("Location is required.", nameof(location));

        Name = name.Trim();
        Location = location.Trim();
        IsActive = true;
    }

    public void Update(String name, String location)
    {
        if (String.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));

        if (String.IsNullOrWhiteSpace(location))
            throw new ArgumentException("Location is required.", nameof(location));

        Name = name.Trim();
        Location = location.Trim();
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }
















}






