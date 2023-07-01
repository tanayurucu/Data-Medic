using DataMedic.Application.Common.Models;
using DataMedic.Domain.Hosts;
using DataMedic.Domain.Hosts.ValueObjects;

namespace DataMedic.Application.Common.Interfaces.Persistence.Repositories;

public interface IHostRepository : IAsyncRepository<Host, HostId>
{
    Task<Host?> FindByNameAsync(HostName name, CancellationToken cancellationToken = default);

    Task<List<Host>> FindAllByHostTypeAsync(
        HostType? hostType,
        CancellationToken cancellationToken = default
    );

    Task<Paged<Host>> FindManyWithPaginationAndFiltersAsync(
        HostType? hostType,
        string searchString,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken = default
    );
}
