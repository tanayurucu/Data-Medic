using DataMedic.Domain.Common.Abstractions;

namespace DataMedic.Application.Common.Interfaces.Persistence.Repositories;

public interface IAsyncRepository<TEntity, in TId>
    where TEntity : AggregateRoot<TId>
    where TId : notnull
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task AddRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default
    );

    void Update(TEntity entity);

    void Remove(TEntity entity);

    void RemoveRange(IEnumerable<TEntity> entities);

    Task<TEntity?> FindByIdAsync(TId id, CancellationToken cancellationToken = default);

    Task<List<TEntity>> FindAllAsync(CancellationToken cancellationToken = default);
}