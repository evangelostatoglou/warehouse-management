using WM.Domain.Entities;

namespace WarehouseManagement.Tests;

public class InventoryTests
{
    [Fact]
    public void Reserve_WhenEnoughStockExists_IncreasesReservedQuantity()
    {
        var inventory = new Inventory(1, 1, 10, 3);

        inventory.Reserve(5);

        Assert.Equal(10, inventory.QuantityOnHand);
        Assert.Equal(8, inventory.QuantityReserved);
    }

    [Fact]
    public void Reserve_WhenAvailableStockIsInsufficient_ThrowsException()
    {
        var inventory = new Inventory(1, 1, 10, 3);

        Assert.Throws<InvalidOperationException>(
            () => inventory.Reserve(8));
    }

    [Fact]
    public void Adjust_WhenQuantityWouldBecomeLowerThanReserved_ThrowsException()
    {
        var inventory = new Inventory(1, 1, 10, 3);

        Assert.Throws<ArgumentException>(
            () => inventory.Adjust(-8));
    }

    [Fact]
    public void Ship_WhenStockIsReserved_ReducesOnHandAndReservedQuantity()
    {
        var inventory = new Inventory(1, 1, 10, 5);

        inventory.Ship(4);

        Assert.Equal(6, inventory.QuantityOnHand);
        Assert.Equal(1, inventory.QuantityReserved);
    }
}