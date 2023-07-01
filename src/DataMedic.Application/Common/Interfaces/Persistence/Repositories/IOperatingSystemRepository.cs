using DataMedic.Application.Common.Models;
using DataMedic.Domain.OperatingSystems.ValueObjects;

using OperatingSystem = DataMedic.Domain.OperatingSystems.OperatingSystem;

namespace DataMedic.Application.Common.Interfaces.Persistence.Repositories;

public interface IOperatingSystemRepository : IAsyncRepository<OperatingSystem, OperatingSystemId>
{
    Task<OperatingSystem?> FindByNameAsync(
        OperatingSystemName name,
        CancellationToken cancellationToken = default
    );

    Task<Paged<OperatingSystem>> FindManyWithPaginationAsync(
        string searchString,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken = default
    );
}
