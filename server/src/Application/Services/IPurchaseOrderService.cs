

using WM.Application.DTOs;

namespace WM.Application.Services;

public interface IPurchaseOrderService
{
    Task<IReadOnlyList<GetPurchaseOrderResponse>> GetAllPurchaseOrdersAsync();
    Task<GetPurchaseOrderResponse?> GetPurchaseOrderByIdAsync(int id);
    Task<GetPurchaseOrderResponse?> CreatePurchaseOrderAsync(CreatePurchaseOrderRequest purchaseOrder);
    Task<GetPurchaseOrderResponse?> ReceivePurchaseOrderAsync(int id);
}
