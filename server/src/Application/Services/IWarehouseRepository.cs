using WM.Domain.Entities;

namespace WM.Application.Services;


public interface IWarehouseRepository
{
    Task<IReadOnlyList<Warehouse>> GetAllWarehousesAsync();
}











