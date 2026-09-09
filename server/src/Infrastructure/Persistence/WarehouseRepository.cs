using Microsoft.EntityFrameworkCore;
using WM.Application.DTOs;
using WM.Application.Services;
using WM.Domain.Entities;
using WM.Infrastructure.Persistence;


namespace WM.Infrastructure.Persistence;

public class WarehouseRepository: IWarehouseRepository
{
    private readonly AppDbContext _dbContext;

    public WarehouseRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Warehouse>> GetAllWarehousesAsync()
    {
        return await _dbContext.Warehouses
            .AsNoTracking()
            .OrderBy(w => w.Name)
            .ToListAsync();
    }
}