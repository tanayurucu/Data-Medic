using DataMedic.Application.Common.Interfaces.Persistence;

using Microsoft.EntityFrameworkCore.Storage;

namespace DataMedic.Persistence;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly DataMedicDbContext _dbContext;

    public UnitOfWork(DataMedicDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }
}