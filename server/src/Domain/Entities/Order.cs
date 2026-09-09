namespace WM.Domain.Entities;

public class Order
{
    public int Id { get; private set; }
    public int CustomerId { get; private set; }
    public String Status { get; private set; } = String.Empty;
    public int CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public decimal TotalAmount { get; private set; }

    private Order() { }

    public Order(int customerId, int createdBy)
    {
        if (customerId <= 0)
            throw new ArgumentException("Customer ID must be greater than zero.", nameof(customerId));

        if (createdBy <= 0)
            throw new ArgumentException("Creator ID must be greater than zero.", nameof(createdBy));

        CustomerId = customerId;
        CreatedBy = createdBy;
        Status = "DRAFT";
        CreatedAt = DateTime.UtcNow;
        TotalAmount = 0;
    }
}
