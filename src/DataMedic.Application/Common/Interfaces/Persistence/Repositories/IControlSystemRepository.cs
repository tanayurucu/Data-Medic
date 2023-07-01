using DataMedic.Application.Common.Models;
using DataMedic.Domain.ControlSystems;
using DataMedic.Domain.ControlSystems.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.Common.Interfaces.Persistence.Repositories;

public interface IControlSystemRepository : IAsyncRepository<ControlSystem, ControlSystemId>
{
    Task<ControlSystem?> FindByNameAsync(
        ControlSystemName name,
        CancellationToken cancellationToken = default
    );
    Task<Paged<ControlSystem>> FindManyWithPaginationAsync(
        string requestSearchString,
        int requestPageSize,
        int requestPageNumber,
        CancellationToken cancellationToken
    );
}