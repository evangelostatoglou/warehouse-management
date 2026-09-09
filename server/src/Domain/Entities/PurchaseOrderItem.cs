namespace WM.Domain.Entities;

public class PurchaseOrderItem
{
    public int Id { get; private set; }
    public int PurchaseOrderId { get; private set; }
    public int ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitCost { get; private set; }

    private PurchaseOrderItem() { }

    public PurchaseOrderItem(int purchaseOrderId, int productId, int quantity, decimal unitCost)
    {
        if (purchaseOrderId <= 0)
            throw new ArgumentException("Purchase order ID must be greater than zero.", nameof(purchaseOrderId));

        if (productId <= 0)
            throw new ArgumentException("Product ID must be greater than zero.", nameof(productId));

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

        if (unitCost < 0)
            throw new ArgumentException("Unit cost cannot be negative.", nameof(unitCost));

        PurchaseOrderId = purchaseOrderId;
        ProductId = productId;
        Quantity = quantity;
        UnitCost = unitCost;
    }
}
