

using WM.Application.DTOs;
using WM.Domain.Entities;

namespace WM.Application.Services;
 
public interface IOrderService
{
    Task<IReadOnlyList<GetOrderResponse>> GetAllOrdersAsync();
    Task<GetOrderResponse?> GetOrderByIdAsync(int id);
    Task<GetOrderResponse?> CreateOrderAsync(CreateOrderRequest order);
    Task<GetOrderResponse?> ConfirmOrderAsync(int id);
    Task<GetOrderResponse?> CancelOrderAsync(int id);
    Task<GetOrderResponse?> ShipOrderAsync(int id);
}

