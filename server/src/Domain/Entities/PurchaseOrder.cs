namespace WM.Domain.Entities;

public class PurchaseOrder
{
    public int Id { get; private set; }
    public int SupplierId { get; private set; }
    public int WarehouseId { get; private set; }
    public String Status { get; private set; } = String.Empty;
    public DateTime CreatedAt { get; private set; }
    public int CreatedBy { get; private set; }
    public List<PurchaseOrderItem> Items { get; private set; } = [];

    private PurchaseOrder() { }

    public PurchaseOrder(int supplierId, int warehouseId, int createdBy)
    {
        if (supplierId <= 0)
            throw new ArgumentException("Supplier ID must be greater than zero.", nameof(supplierId));

        if (warehouseId <= 0)
            throw new ArgumentException("Warehouse ID must be greater than zero.", nameof(warehouseId));

        if (createdBy <= 0)
            throw new ArgumentException("Creator ID must be greater than zero.", nameof(createdBy));

        SupplierId = supplierId;
        WarehouseId = warehouseId;
        CreatedBy = createdBy;
        Status = "DRAFT";
        CreatedAt = DateTime.UtcNow;
    }

    public void AddItem(int productId, int quantity, decimal unitCost)
    {
        var item = new PurchaseOrderItem(productId, quantity, unitCost);
        Items.Add(item);
    }

    public void Receive()
    {
        if (Status != "DRAFT")
            throw new InvalidOperationException("Only draft purchase orders can be received.");

        Status = "RECEIVED";
    }
}
