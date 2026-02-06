using EquipmentManagement.Domain.Entities;
using EquipmentManagement.Domain.Interfaces;
using EquipmentManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace EquipmentManagement.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;
    private IEquipmentRepository? _equipmentRepository;
    private IGenericRepository<Category>? _categoryRepository;
    private IGenericRepository<Location>? _locationRepository;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEquipmentRepository Equipment
    {
        get
        {
            _equipmentRepository ??= new EquipmentRepository(_context);
            return _equipmentRepository;
        }
    }

    public IGenericRepository<Category> Categories
    {
        get
        {
            _categoryRepository ??= new GenericRepository<Category>(_context);
            return _categoryRepository;
        }
    }

    public IGenericRepository<Location> Locations
    {
        get
        {
            _locationRepository ??= new GenericRepository<Location>(_context);
            return _locationRepository;
        }
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
