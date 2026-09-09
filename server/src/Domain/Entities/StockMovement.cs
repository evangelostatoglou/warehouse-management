namespace WM.Domain.Entities;

public class StockMovement
{
    public int Id { get; private set; }
    public int ProductId { get; private set; }
    public int WarehouseId { get; private set; }
    public String Type { get; private set; } = String.Empty;
    public int Quantity { get; private set; }
    public int? ReferenceId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public int CreatedBy { get; private set; }

    private StockMovement() { }

    public StockMovement(int productId, int warehouseId, String type, int quantity, int? referenceId, int createdBy)
    {
        if (productId <= 0)
            throw new ArgumentException("Product ID must be greater than zero.", nameof(productId));

        if (warehouseId <= 0)
            throw new ArgumentException("Warehouse ID must be greater than zero.", nameof(warehouseId));

        if (String.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Movement type is required.", nameof(type));

        type = type.Trim().ToUpperInvariant();

        if (type is not ("PURCHASE" or "SALE" or "RETURN" or "TRANSFER_IN" or "TRANSFER_OUT" or "ADJUSTMENT"))
            throw new ArgumentException("Movement type is invalid.", nameof(type));

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

        if (createdBy <= 0)
            throw new ArgumentException("Creator ID must be greater than zero.", nameof(createdBy));

        ProductId = productId;
        WarehouseId = warehouseId;
        Type = type;
        Quantity = quantity;
        ReferenceId = referenceId;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
    }
}
