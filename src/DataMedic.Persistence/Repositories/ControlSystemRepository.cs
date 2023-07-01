using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Models;
using DataMedic.Domain.ControlSystems;
using DataMedic.Domain.ControlSystems.ValueObjects;
using DataMedic.Persistence.Common.Abstractions;
using DataMedic.Persistence.Common.Extensions;

using Microsoft.EntityFrameworkCore;

namespace DataMedic.Persistence.Repositories;

internal sealed class ControlSystemRepository : AsyncRepository<ControlSystem, ControlSystemId>, IControlSystemRepository
{
    public ControlSystemRepository(DataMedicDbContext dbContext) : base(dbContext)
    {
    }

    public Task<ControlSystem?> FindByNameAsync(ControlSystemName name, CancellationToken cancellationToken = default)
    {
        return (from controlSystem in _dbContext.Set<ControlSystem>()
                where controlSystem.Name == name
                select controlSystem).FirstOrDefaultAsync(cancellationToken);
    }

    public Task<Paged<ControlSystem>> FindManyWithPaginationAsync(string requestSearchString, int requestPageSize, int requestPageNumber,
        CancellationToken cancellationToken)
    {
        return (from controlSystem in _dbContext.Set<ControlSystem>()
                where string.IsNullOrEmpty(requestSearchString) || ((string)controlSystem.Name).Contains(requestSearchString)
                orderby controlSystem.Name
                select controlSystem).ToPagedListAsync(requestPageNumber, requestPageSize, cancellationToken);
    }
}