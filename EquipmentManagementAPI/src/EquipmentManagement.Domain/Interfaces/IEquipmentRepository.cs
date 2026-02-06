using EquipmentManagement.Domain.Entities;

namespace EquipmentManagement.Domain.Interfaces;

public interface IEquipmentRepository : IGenericRepository<Equipment>
{
    Task<(IEnumerable<Equipment> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        int? categoryId = null,
        EquipmentStatus? status = null,
        string sortBy = "EquipmentName",
        string sortDirection = "ASC");

    Task<IEnumerable<Equipment>> GetByCategoryAsync(int categoryId);
    Task<Equipment?> GetBySerialNumberAsync(string serialNumber);
    Task<bool> SerialNumberExistsAsync(string serialNumber, int? excludeEquipmentId = null);
}
