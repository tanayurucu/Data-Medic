using System.Text.Json;

using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Models;
using DataMedic.Domain.Hosts;
using DataMedic.Domain.Hosts.ValueObjects;
using DataMedic.Persistence.Common.Abstractions;
using DataMedic.Persistence.Common.Extensions;

using Microsoft.EntityFrameworkCore;

namespace DataMedic.Persistence.Repositories;

internal sealed class HostRepository : AsyncRepository<Host, HostId>, IHostRepository
{
    public HostRepository(DataMedicDbContext dbContext)
        : base(dbContext) { }

    public Task<List<Host>> FindAllByHostTypeAsync(
        HostType? hostType,
        CancellationToken cancellationToken = default
    ) =>
        (
            from host in _dbContext.Set<Host>()
            where hostType == null || host.Type == hostType
            select host
        ).ToListAsync(cancellationToken);

    public Task<Host?> FindByNameAsync(
        HostName name,
        CancellationToken cancellationToken = default
    ) => _dbContext.Set<Host>().FirstOrDefaultAsync(host => host.Name == name, cancellationToken);

    public Task<Paged<Host>> FindManyWithPaginationAndFiltersAsync(
        HostType? hostType,
        string searchString,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken = default
    ) =>
       (
            from host in _dbContext.Set<Host>()
            where hostType == null || host.Type == hostType
            where
                string.IsNullOrWhiteSpace(searchString)
                || ((string)host.Name).Contains(searchString)
            select host
        ).ToPagedListAsync(pageNumber, pageSize, cancellationToken);
}
