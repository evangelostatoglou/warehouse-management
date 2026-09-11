using WM.Application.DTOs;
using WM.Domain.Entities;

namespace WM.Application.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _supplierRepository;

    public SupplierService(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<IReadOnlyList<GetSupplierResponse>> GetAllSuppliersAsync()
    {
        var suppliers = await _supplierRepository.GetAllSuppliersAsync();

        return suppliers.Select(supplier => new GetSupplierResponse
        {
            Id = supplier.Id,
            Name = supplier.Name,
            Email = supplier.Email,
            IsActive = supplier.IsActive
        }).ToList();
    }

    public async Task<GetSupplierResponse?> GetSupplierByIdAsync(int id)
    {
        var supplier = await _supplierRepository.GetSupplierByIdAsync(id);
        if(supplier is null) return null;

        return new GetSupplierResponse
        {
            Id = supplier.Id,
            Name = supplier.Name,
            Email = supplier.Email,
            IsActive = supplier.IsActive
        };
    }

    public async Task<GetSupplierResponse> CreateSupplierAsync(CreateSupplierRequest supplier)
    {
        var s = new Supplier(supplier.Name, supplier.Email);

        await _supplierRepository.CreateSupplierAsync(s);

        return new GetSupplierResponse
        {
            Id = s.Id,
            Name = s.Name,
            Email = s.Email,
            IsActive = s.IsActive
        };
    }

    public async Task<GetSupplierResponse?> UpdateSupplierAsync(UpdateSupplierRequest supplier, int id)
    {
        var s = await _supplierRepository.GetSupplierByIdAsync(id);
        if(s is null) return null;

        s.Update(supplier.Name, supplier.Email);

        await _supplierRepository.UpdateSupplierAsync(s);

        return new GetSupplierResponse
        {
            Id = s.Id,
            Name = s.Name,
            Email = s.Email,
            IsActive = s.IsActive
        };
    }

    public async Task<GetSupplierResponse?> UpdateSupplierStatusAsync(UpdateSupplierStatusRequest status, int id)
    {
        if(status.IsActive is null) return null;

        var s = await _supplierRepository.GetSupplierByIdAsync(id);
        if(s is null) return null;

        if(status.IsActive == true) s.Activate();
        else s.Deactivate();

        await _supplierRepository.UpdateSupplierAsync(s);

        return new GetSupplierResponse
        {
            Id = s.Id,
            Name = s.Name,
            Email = s.Email,
            IsActive = s.IsActive
        };
    }
}
