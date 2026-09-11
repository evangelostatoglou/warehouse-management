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

    public async Task<Warehouse?> GetWarehouseByIdAsync(int id)
    {
        return await _dbContext.Warehouses
            .AsNoTracking()
            .SingleOrDefaultAsync(w => w.Id == id);
    }

    public async Task CreateWarehouseAsync(Warehouse warehouse)
    {
        _dbContext.Warehouses.Add(warehouse);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateWarehouseAsync(Warehouse warehouse)
    {
        _dbContext.Warehouses.Update(warehouse);
        await _dbContext.SaveChangesAsync();
    }
}
