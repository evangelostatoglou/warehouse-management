using WM.Application.DTOs;


namespace WM.Application.Services;

public interface IWarehouseService
{
    Task<IReadOnlyList<GetWarehouseResponse>> GetAllWarehousesAsync();
}













