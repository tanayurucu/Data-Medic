using DataMedic.Application.Common.Models;
using DataMedic.Application.Devices.Models;
using DataMedic.Domain.ControlSystems.ValueObjects;
using DataMedic.Domain.Departments.ValueObjects;
using DataMedic.Domain.DeviceGroups.ValueObjects;
using DataMedic.Domain.Devices;
using DataMedic.Domain.Devices.Entities;
using DataMedic.Domain.Devices.ValueObjects;
using DataMedic.Domain.OperatingSystems.ValueObjects;

namespace DataMedic.Application.Common.Interfaces.Persistence.Repositories;

public interface IDeviceRepository : IAsyncRepository<Device, DeviceId>
{
    Task<Device?> FindByNameAsync(DeviceName name, CancellationToken cancellationToken = default);
    Task<Device?> FindByInventoryNumber(
        InventoryNumber inventoryNumber,
        CancellationToken cancellationToken = default
    );
    Task<Paged<DeviceWithDetails>> FindManyWithPaginationAsync(
        string searchString,
        int pageSize,
        int pageNumber,
        DepartmentId departmentId,
        DeviceGroupId deviceGroupId,
        CancellationToken cancellationToken = default
    );

    Task<List<DeviceWithDetails>> FindAllWithDetailsAsync(
        CancellationToken cancellationToken = default
    );

    Task<Paged<DeviceComponentWithDetails>> FindDeviceComponentsWithPaginationAsync(
        string searchString,
        int pageSize,
        int pageNumber,
        DeviceId deviceId,
        OperatingSystemId operatingSystemId,
        ControlSystemId controlSystemId,
        CancellationToken cancellationToken = default
    );

    Task<List<DeviceComponentWithDetails>> FindAllDeviceComponentsWithDetailsForDeviceAsync(
        DeviceId deviceId,
        CancellationToken cancellationToken = default
    );
}
