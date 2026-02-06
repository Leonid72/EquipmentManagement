namespace EquipmentManagement.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IEquipmentRepository Equipment { get; }
    IGenericRepository<Entities.Category> Categories { get; }
    IGenericRepository<Entities.Location> Locations { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
