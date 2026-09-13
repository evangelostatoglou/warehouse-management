using WM.Domain.Entities;

namespace WM.Application.Services;

public interface IOrderRepository
{
    Task<IReadOnlyList<Order>> GetAllOrdersAsync();
    Task<Order?> GetOrderByIdAsync(int id);
    Task CreateOrderAsync(Order order);
    Task<Order?> ConfirmOrderAsync(int id);
    Task<Order?> CancelOrderAsync(int id);
    Task<Order?> ShipOrderAsync(int id);
}
 