using WM.Application.DTOs;


namespace WM.Application.Services;

public interface IWarehouseService
{
    Task<IReadOnlyList<GetWarehouseResponse>> GetAllWarehousesAsync();
    Task<GetWarehouseResponse?> GetWarehouseByIdAsync(int id);
    Task<GetWarehouseResponse> CreateWarehouseAsync(CreateWarehouseRequest warehouse);
    Task<GetWarehouseResponse?> UpdateWarehouseAsync(UpdateWarehouseRequest warehouse, int id);
    Task<GetWarehouseResponse?> UpdateWarehouseStatusAsync(UpdateWarehouseStatusRequest status, int id);
}













