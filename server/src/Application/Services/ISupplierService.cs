using WM.Application.DTOs;

namespace WM.Application.Services;

public interface ISupplierService
{
    Task<IReadOnlyList<GetSupplierResponse>> GetAllSuppliersAsync();
    Task<GetSupplierResponse?> GetSupplierByIdAsync(int id);
    Task<GetSupplierResponse> CreateSupplierAsync(CreateSupplierRequest supplier);
    Task<GetSupplierResponse?> UpdateSupplierAsync(UpdateSupplierRequest supplier, int id);
    Task<GetSupplierResponse?> UpdateSupplierStatusAsync(UpdateSupplierStatusRequest status, int id);
}
