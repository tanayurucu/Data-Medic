using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Models;
using DataMedic.Domain.OperatingSystems.ValueObjects;
using DataMedic.Persistence.Common.Abstractions;
using DataMedic.Persistence.Common.Extensions;

using Microsoft.EntityFrameworkCore;

using OperatingSystem = DataMedic.Domain.OperatingSystems.OperatingSystem;

namespace DataMedic.Persistence.Repositories;

internal sealed class OperatingSystemRepository : AsyncRepository<OperatingSystem, OperatingSystemId>, IOperatingSystemRepository
{
    public OperatingSystemRepository(DataMedicDbContext dbContext) : base(dbContext)
    {
    }

    public Task<OperatingSystem?> FindByNameAsync(OperatingSystemName name, CancellationToken cancellationToken = default)
    {
        return (from operatingSystem in _dbContext.Set<OperatingSystem>()
                where operatingSystem.Name == name
                select operatingSystem).FirstOrDefaultAsync(cancellationToken);
    }

    public Task<Paged<OperatingSystem>> FindManyWithPaginationAsync(string searchString, int pageSize, int pageNumber,
        CancellationToken cancellationToken = default)
    {
        return (from operatingSystem in _dbContext.Set<OperatingSystem>()
                where string.IsNullOrEmpty(searchString) || ((string)operatingSystem.Name).Contains(searchString)
                orderby operatingSystem.Name
                select operatingSystem).ToPagedListAsync(pageNumber, pageSize, cancellationToken);
    }
}