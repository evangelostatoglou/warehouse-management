

using WM.Application.DTOs;
using WM.Domain.Entities;


namespace WM.Application.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepository;

    public WarehouseService(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<IReadOnlyList<GetWarehouseResponse>> GetAllWarehousesAsync()
    {
        var warehouses = await _warehouseRepository.GetAllWarehousesAsync();
        return warehouses.Select(warehouse => new GetWarehouseResponse
        {
            Id = warehouse.Id,
            Name = warehouse.Name,
            Location = warehouse.Location,
            IsActive = warehouse.IsActive
        }).ToList();
    }


    public async Task<GetWarehouseResponse?> GetWarehouseByIdAsync(int id)
    {
        var warehouse = await _warehouseRepository.GetWarehouseByIdAsync(id);
        if(warehouse is null) return null;

        return new GetWarehouseResponse
        {
            Id = warehouse.Id,
            Name = warehouse.Name,
            Location = warehouse.Location,
            IsActive = warehouse.IsActive
        };
    }

    public async Task<GetWarehouseResponse> CreateWarehouseAsync(CreateWarehouseRequest warehouse)
    {
        var w = new Warehouse(warehouse.Name, warehouse.Location);

        await _warehouseRepository.CreateWarehouseAsync(w);

        return new GetWarehouseResponse
        {
            Id = w.Id,
            Name = w.Name,
            Location = w.Location,
            IsActive = w.IsActive
        };
    }

    public async Task<GetWarehouseResponse?> UpdateWarehouseAsync(UpdateWarehouseRequest warehouse, int id)
    {
        var w = await _warehouseRepository.GetWarehouseByIdAsync(id);
        if(w is null) return null;

        w.Update(warehouse.Name, warehouse.Location);

        await _warehouseRepository.UpdateWarehouseAsync(w);

        return new GetWarehouseResponse
        {
            Id = w.Id,
            Name = w.Name,
            Location = w.Location,
            IsActive = w.IsActive
        };
    }

    public async Task<GetWarehouseResponse?> UpdateWarehouseStatusAsync(UpdateWarehouseStatusRequest status, int id)
    {
        if(status.IsActive is null) return null;

        var w = await _warehouseRepository.GetWarehouseByIdAsync(id);
        if(w is null) return null;

        if(status.IsActive == true) w.Activate();
        else w.Deactivate();

        await _warehouseRepository.UpdateWarehouseAsync(w);

        return new GetWarehouseResponse
        {
            Id = w.Id,
            Name = w.Name,
            Location = w.Location,
            IsActive = w.IsActive
        };
    }

}


