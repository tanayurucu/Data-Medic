using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Models;
using DataMedic.Domain.DeviceGroups;
using DataMedic.Domain.DeviceGroups.ValueObjects;
using DataMedic.Persistence.Common.Abstractions;
using DataMedic.Persistence.Common.Extensions;

using Microsoft.EntityFrameworkCore;

namespace DataMedic.Persistence.Repositories;

internal sealed class DeviceGroupRepository : AsyncRepository<DeviceGroup, DeviceGroupId>, IDeviceGroupRepository
{
    public DeviceGroupRepository(DataMedicDbContext dbContext) : base(dbContext)
    {
    }

    public Task<DeviceGroup?> FindByNameAsync(DeviceGroupName name, CancellationToken cancellationToken = default)
    {
        return (from deviceGroup in _dbContext.Set<DeviceGroup>()
                where deviceGroup.Name == name
                select deviceGroup).FirstOrDefaultAsync(cancellationToken);
    }

    public Task<Paged<DeviceGroup>> FindManyWithPaginationAsync(string searchString, int pageSize, int pageNumber,
        CancellationToken cancellationToken = default)
    {
        return (from deviceGroup in _dbContext.Set<DeviceGroup>()
                where string.IsNullOrEmpty(searchString) || ((string)deviceGroup.Name).Contains(searchString)
                orderby deviceGroup.Name
                select deviceGroup).ToPagedListAsync(pageNumber, pageSize, cancellationToken);
    }
}