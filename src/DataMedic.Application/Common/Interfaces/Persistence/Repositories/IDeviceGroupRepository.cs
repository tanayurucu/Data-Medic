using DataMedic.Application.Common.Models;
using DataMedic.Domain.DeviceGroups;
using DataMedic.Domain.DeviceGroups.ValueObjects;

namespace DataMedic.Application.Common.Interfaces.Persistence.Repositories;

public interface IDeviceGroupRepository : IAsyncRepository<DeviceGroup, DeviceGroupId>
{
    Task<DeviceGroup?> FindByNameAsync(
        DeviceGroupName name,
        CancellationToken cancellationToken = default
    );

    Task<Paged<DeviceGroup>> FindManyWithPaginationAsync(
        string searchString,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken = default
    );
}