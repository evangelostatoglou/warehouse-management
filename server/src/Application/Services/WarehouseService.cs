

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
            Location = warehouse.Location
        }).ToList();

    }


}


