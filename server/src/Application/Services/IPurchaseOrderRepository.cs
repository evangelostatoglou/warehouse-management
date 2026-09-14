using WM.Domain.Entities;

namespace WM.Application.Services;

public interface IPurchaseOrderRepository
{
    Task<IReadOnlyList<PurchaseOrder>> GetAllPurchaseOrdersAsync();
    Task<PurchaseOrder?> GetPurchaseOrderByIdAsync(int id);
    Task CreatePurchaseOrderAsync(PurchaseOrder purchaseOrder);
    Task<PurchaseOrder?> ReceivePurchaseOrderAsync(int id);
}
