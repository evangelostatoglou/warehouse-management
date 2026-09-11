using WM.Application.DTOs;
using WM.Domain.Entities;

namespace WM.Application.Services;


public interface IWarehouseRepository
{
    Task<IReadOnlyList<Warehouse>> GetAllWarehousesAsync();
    Task<Warehouse?> GetWarehouseByIdAsync(int id);
    Task CreateWarehouseAsync(Warehouse warehouse);
    Task UpdateWarehouseAsync(Warehouse warehouse);
}











