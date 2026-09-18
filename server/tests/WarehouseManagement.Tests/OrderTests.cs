using WM.Domain.Entities;

namespace WarehouseManagement.Tests;

public class OrderTests
{
    [Fact]
    public void Confirm_WhenOrderIsDraft_ChangesStatusToConfirmed()
    {
        var order = new Order(1, 1, 1);

        order.Confirm();

        Assert.Equal("CONFIRMED", order.Status);
    }

    [Fact]
    public void Ship_WhenOrderIsConfirmed_ChangesStatusToShipped()
    {
        var order = new Order(1, 1, 1);
        order.Confirm();

        order.Ship();

        Assert.Equal("SHIPPED", order.Status);
    }

    [Fact]
    public void Ship_WhenOrderIsDraft_ThrowsException()
    {
        var order = new Order(1, 1, 1);

        Assert.Throws<InvalidOperationException>(
            () => order.Ship());
    }

    [Fact]
    public void Cancel_WhenOrderIsConfirmed_ChangesStatusToCancelled()
    {
        var order = new Order(1, 1, 1);
        order.Confirm();

        order.Cancel();

        Assert.Equal("CANCELLED", order.Status);
    }

    [Fact]
    public void Cancel_WhenOrderIsShipped_ThrowsException()
    {
        var order = new Order(1, 1, 1);
        order.Confirm();
        order.Ship();

        Assert.Throws<InvalidOperationException>(
            () => order.Cancel());
    }
}