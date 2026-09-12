namespace WM.Domain.Entities;

public class Inventory
{
    public int Id { get; private set; }
    public int ProductId { get; private set; }
    public int WarehouseId { get; private set; }
    public int QuantityOnHand { get; private set; }
    public int QuantityReserved { get; private set; }

    private Inventory() { }

    public Inventory(int productId, int warehouseId, int quantityOnHand, int quantityReserved)
    {
        if (productId <= 0)
            throw new ArgumentException("Product ID must be greater than zero.", nameof(productId));

        if (warehouseId <= 0)
            throw new ArgumentException("Warehouse ID must be greater than zero.", nameof(warehouseId));

        if (quantityOnHand < 0)
            throw new ArgumentException("Quantity on hand cannot be negative.", nameof(quantityOnHand));

        if (quantityReserved < 0 || quantityReserved > quantityOnHand)
            throw new ArgumentException("Reserved quantity must be between zero and quantity on hand.", nameof(quantityReserved));

        ProductId = productId;
        WarehouseId = warehouseId;
        QuantityOnHand = quantityOnHand;
        QuantityReserved = quantityReserved;
    }

    public void Adjust(int quantityChange)
    {
        if (quantityChange == 0)
            throw new ArgumentException("Quantity change cannot be zero.", nameof(quantityChange));

        int newQuantityOnHand = QuantityOnHand + quantityChange;

        if (newQuantityOnHand < QuantityReserved)
            throw new ArgumentException("Quantity on hand cannot be lower than reserved quantity.", nameof(quantityChange));

        QuantityOnHand = newQuantityOnHand;
    }
}
