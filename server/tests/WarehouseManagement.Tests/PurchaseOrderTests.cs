using WM.Domain.Entities;

namespace WarehouseManagement.Tests;

public class PurchaseOrderTests
{
    [Fact]
    public void CreatePurchaseOrder_SetsStatusToDraft()
    {
        var purchaseOrder = new PurchaseOrder(
            supplierId: 1,
            warehouseId: 1,
            createdBy: 1);

        Assert.Equal("DRAFT", purchaseOrder.Status);
    }

    [Fact]
    public void Receive_WhenPurchaseOrderIsDraft_ChangesStatusToReceived()
    {
        var purchaseOrder = new PurchaseOrder(1, 1, 1);

        purchaseOrder.Receive();

        Assert.Equal("RECEIVED", purchaseOrder.Status);
    }

    [Fact]
    public void Receive_WhenPurchaseOrderIsAlreadyReceived_ThrowsException()
    {
        var purchaseOrder = new PurchaseOrder(1, 1, 1);
        purchaseOrder.Receive();

        Assert.Throws<InvalidOperationException>(
            () => purchaseOrder.Receive());
    }

    [Fact]
    public void AddItem_AddsItemToPurchaseOrder()
    {
        var purchaseOrder = new PurchaseOrder(1, 1, 1);

        purchaseOrder.AddItem(
            productId: 5,
            quantity: 10,
            unitCost: 12.50m);

        Assert.Single(purchaseOrder.Items);

        var item = purchaseOrder.Items[0];

        Assert.Equal(5, item.ProductId);
        Assert.Equal(10, item.Quantity);
        Assert.Equal(12.50m, item.UnitCost);
    }
}