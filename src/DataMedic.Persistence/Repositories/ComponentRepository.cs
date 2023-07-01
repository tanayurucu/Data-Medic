using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Models;
using DataMedic.Domain.Components;
using DataMedic.Domain.Components.ValueObjects;
using DataMedic.Persistence.Common.Abstractions;
using DataMedic.Persistence.Common.Extensions;

using Microsoft.EntityFrameworkCore;

namespace DataMedic.Persistence.Repositories;

internal sealed class ComponentRepository
    : AsyncRepository<Component, ComponentId>,
        IComponentRepository
{
    public ComponentRepository(DataMedicDbContext dbContext)
        : base(dbContext) { }

    public Task<Component?> FindByNameAsync(
        ComponentName name,
        CancellationToken cancellationToken = default
    ) =>
        (
            from component in _dbContext.Set<Component>()
            where component.Name == name
            select component
        ).FirstOrDefaultAsync(cancellationToken);

    public Task<Paged<Component>> FindManyWithPaginationAsync(
        string searchString,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken = default
    ) =>
        (
            from component in _dbContext.Set<Component>()
            where
                string.IsNullOrWhiteSpace(searchString)
                || ((string)component.Name).Contains(searchString)
            orderby component.Name
            select component
        ).ToPagedListAsync(pageNumber, pageSize, cancellationToken);
}
