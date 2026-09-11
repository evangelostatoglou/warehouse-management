using WM.Domain.Entities;

namespace WM.Application.Services;

public interface ISupplierRepository
{
    Task<IReadOnlyList<Supplier>> GetAllSuppliersAsync();
    Task<Supplier?> GetSupplierByIdAsync(int id);
    Task CreateSupplierAsync(Supplier supplier);
    Task UpdateSupplierAsync(Supplier supplier);
}
