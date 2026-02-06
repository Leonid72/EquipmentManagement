using EquipmentManagement.Domain.Entities;
using EquipmentManagement.Domain.Interfaces;
using EquipmentManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EquipmentManagement.Infrastructure.Repositories;

public class EquipmentRepository : GenericRepository<Equipment>, IEquipmentRepository
{
    public EquipmentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Equipment?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(e => e.Category)
            .Include(e => e.Location)
            .FirstOrDefaultAsync(e => e.EquipmentID == id);
    }

    public override async Task<IEnumerable<Equipment>> GetAllAsync()
    {
        return await _dbSet
            .Include(e => e.Category)
            .Include(e => e.Location)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Equipment> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        int? categoryId = null,
        EquipmentStatus? status = null,
        string sortBy = "EquipmentName",
        string sortDirection = "ASC")
    {
        var query = _dbSet
            .Include(e => e.Category)
            .Include(e => e.Location)
            .AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(e =>
                e.EquipmentName.Contains(searchTerm) ||
                e.SerialNumber.Contains(searchTerm));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(e => e.CategoryID == categoryId.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(e => e.Status == status.Value);
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync();

        // Apply sorting
        query = sortBy.ToLower() switch
        {
            "equipmentname" => sortDirection.ToUpper() == "DESC"
                ? query.OrderByDescending(e => e.EquipmentName)
                : query.OrderBy(e => e.EquipmentName),
            "purchasedate" => sortDirection.ToUpper() == "DESC"
                ? query.OrderByDescending(e => e.PurchaseDate)
                : query.OrderBy(e => e.PurchaseDate),
            "status" => sortDirection.ToUpper() == "DESC"
                ? query.OrderByDescending(e => e.Status)
                : query.OrderBy(e => e.Status),
            "categoryname" => sortDirection.ToUpper() == "DESC"
                ? query.OrderByDescending(e => e.Category!.CategoryName)
                : query.OrderBy(e => e.Category!.CategoryName),
            _ => query.OrderBy(e => e.EquipmentName)
        };

        // Apply pagination
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<IEnumerable<Equipment>> GetByCategoryAsync(int categoryId)
    {
        return await _dbSet
            .Include(e => e.Category)
            .Include(e => e.Location)
            .Where(e => e.CategoryID == categoryId)
            .ToListAsync();
    }

    public async Task<Equipment?> GetBySerialNumberAsync(string serialNumber)
    {
        return await _dbSet
            .Include(e => e.Category)
            .Include(e => e.Location)
            .FirstOrDefaultAsync(e => e.SerialNumber == serialNumber);
    }

    public async Task<bool> SerialNumberExistsAsync(string serialNumber, int? excludeEquipmentId = null)
    {
        var query = _dbSet.Where(e => e.SerialNumber == serialNumber);

        if (excludeEquipmentId.HasValue)
        {
            query = query.Where(e => e.EquipmentID != excludeEquipmentId.Value);
        }

        return await query.AnyAsync();
    }
}
