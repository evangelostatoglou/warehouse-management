namespace WM.Domain.Entities;

public class Order
{
    public int Id { get; private set; }
    public int CustomerId { get; private set; }
    public int WarehouseId { get; private set; }
    public String Status { get; private set; } = String.Empty;
    public int CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public decimal TotalAmount { get; private set; }
    public List<OrderItem> Items { get; private set; } = [];

    private Order() { }

    public Order(int customerId, int warehouseId, int createdBy)
    {
        if (customerId <= 0)
            throw new ArgumentException("Customer ID must be greater than zero.", nameof(customerId));

        if (warehouseId <= 0)
            throw new ArgumentException("Warehouse ID must be greater than zero.", nameof(warehouseId));

        if (createdBy <= 0)
            throw new ArgumentException("Creator ID must be greater than zero.", nameof(createdBy));

        CustomerId = customerId;
        WarehouseId = warehouseId;
        CreatedBy = createdBy;
        Status = "DRAFT";
        CreatedAt = DateTime.UtcNow;
        TotalAmount = 0;
    }

    public void AddItem(int productId, int quantity, decimal unitPrice)
    {
        var item = new OrderItem(productId, quantity, unitPrice);

        Items.Add(item);
        TotalAmount += quantity * unitPrice;
    }

    public void Confirm()
    {
        if (Status != "DRAFT")
            throw new InvalidOperationException("Only draft orders can be confirmed.");

        Status = "CONFIRMED";
    }

    public void Cancel()
    {
        if (Status is not ("DRAFT" or "CONFIRMED"))
            throw new InvalidOperationException("Only draft or confirmed orders can be cancelled.");

        Status = "CANCELLED";
    }

    public void Ship()
    {
        if (Status != "CONFIRMED")
            throw new InvalidOperationException("Only confirmed orders can be shipped.");

        Status = "SHIPPED";
    }
}
