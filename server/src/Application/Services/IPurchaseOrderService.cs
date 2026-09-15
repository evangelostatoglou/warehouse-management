

using WM.Application.DTOs;

namespace WM.Application.Services;

public interface IPurchaseOrderService
{
    Task<IReadOnlyList<GetPurchaseOrderResponse>> GetAllPurchaseOrdersAsync();
    Task<GetPurchaseOrderResponse?> GetPurchaseOrderByIdAsync(int id);
    Task<GetPurchaseOrderResponse?> CreatePurchaseOrderAsync(CreatePurchaseOrderRequest purchaseOrder, int createdBy);
    Task<GetPurchaseOrderResponse?> ReceivePurchaseOrderAsync(int id);
}
