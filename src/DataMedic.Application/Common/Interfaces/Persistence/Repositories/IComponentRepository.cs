using DataMedic.Application.Common.Models;
using DataMedic.Domain.Components;
using DataMedic.Domain.Components.ValueObjects;

namespace DataMedic.Application.Common.Interfaces.Persistence.Repositories;

public interface IComponentRepository : IAsyncRepository<Component, ComponentId>
{
    Task<Component?> FindByNameAsync(
        ComponentName name,
        CancellationToken cancellationToken = default
    );

    Task<Paged<Component>> FindManyWithPaginationAsync(
        string searchString,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken = default
    );
}