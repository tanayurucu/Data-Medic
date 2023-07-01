using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Models;
using DataMedic.Application.Devices.Models;
using DataMedic.Domain.Components;
using DataMedic.Domain.ControlSystems;
using DataMedic.Domain.ControlSystems.ValueObjects;
using DataMedic.Domain.Departments;
using DataMedic.Domain.Departments.ValueObjects;
using DataMedic.Domain.DeviceGroups;
using DataMedic.Domain.DeviceGroups.ValueObjects;
using DataMedic.Domain.Devices;
using DataMedic.Domain.Devices.ValueObjects;
using DataMedic.Domain.OperatingSystems.ValueObjects;
using DataMedic.Persistence.Common.Abstractions;
using DataMedic.Persistence.Common.Extensions;

using Microsoft.EntityFrameworkCore;

using OperatingSystem = DataMedic.Domain.OperatingSystems.OperatingSystem;

namespace DataMedic.Persistence.Repositories;

internal sealed class DeviceRepository : AsyncRepository<Device, DeviceId>, IDeviceRepository
{
    public DeviceRepository(DataMedicDbContext dbContext)
        : base(dbContext) { }

    public Task<Device?> FindByNameAsync(
        DeviceName name,
        CancellationToken cancellationToken = default
    ) =>
        (
            from device in _dbContext.Set<Device>()
            where device.Name == name
            select device
        ).FirstOrDefaultAsync(cancellationToken);

    public Task<Device?> FindByInventoryNumber(
        InventoryNumber inventoryNumber,
        CancellationToken cancellationToken = default
    ) =>
        (
            from device in _dbContext.Set<Device>()
            where device.InventoryNumber == inventoryNumber
            select device
        ).FirstOrDefaultAsync(cancellationToken);

    public Task<Paged<DeviceWithDetails>> FindManyWithPaginationAsync(
        string searchString,
        int pageSize,
        int pageNumber,
        DepartmentId departmentId,
        DeviceGroupId deviceGroupId,
        CancellationToken cancellationToken = default
    ) =>
        (
            from device in _dbContext.Set<Device>()
            where
                string.IsNullOrEmpty(searchString)
                || ((string)device.Name).Contains(searchString)
                || ((string)device.InventoryNumber).Contains(searchString)
            where departmentId.Value == Guid.Empty || device.DepartmentId == departmentId
            where deviceGroupId.Value == Guid.Empty || device.DeviceGroupId == deviceGroupId
            join department in _dbContext.Set<Department>()
                on device.DepartmentId equals department.Id
            join deviceGroup in _dbContext.Set<DeviceGroup>()
                on device.DeviceGroupId equals deviceGroup.Id
            orderby device.Name
            select new DeviceWithDetails(
                device.Id,
                device.Name,
                device.Description,
                device.InventoryNumber,
                deviceGroup,
                department
            )
        ).ToPagedListAsync(pageNumber, pageSize, cancellationToken);

    public Task<Paged<DeviceComponentWithDetails>> FindDeviceComponentsWithPaginationAsync(
        string searchString,
        int pageSize,
        int pageNumber,
        DeviceId deviceId,
        OperatingSystemId operatingSystemId,
        ControlSystemId controlSystemId,
        CancellationToken cancellationToken = default
    ) =>
        (
            from device in _dbContext.Set<Device>()
            where device.Id == deviceId
            let deviceComponents = device.Components
            from deviceComponent in deviceComponents
            where
                string.IsNullOrWhiteSpace(searchString)
                || ((string)deviceComponent.IpAddress).Contains(searchString)
            where
                operatingSystemId.Value == Guid.Empty
                || deviceComponent.OperatingSystemId == operatingSystemId
            where
                controlSystemId.Value == Guid.Empty
                || deviceComponent.ControlSystemId == controlSystemId
            join controlSystem in _dbContext.Set<ControlSystem>()
                on deviceComponent.ControlSystemId equals controlSystem.Id
            join operatingSystem in _dbContext.Set<OperatingSystem>()
                on deviceComponent.OperatingSystemId equals operatingSystem.Id
            join component in _dbContext.Set<Component>()
                on deviceComponent.ComponentId equals component.Id
            select new DeviceComponentWithDetails(
                deviceComponent.Id,
                component,
                deviceComponent.IpAddress,
                operatingSystem,
                controlSystem,
                deviceComponent.CreatedOnUtc,
                deviceComponent.ModifiedOnUtc
            )
        ).ToPagedListAsync(pageNumber, pageSize, cancellationToken);

    public Task<List<DeviceWithDetails>> FindAllWithDetailsAsync(
        CancellationToken cancellationToken = default
    ) =>
        (
            from device in _dbContext.Set<Device>()
            join department in _dbContext.Set<Department>()
                on device.DepartmentId equals department.Id
            join deviceGroup in _dbContext.Set<DeviceGroup>()
                on device.DeviceGroupId equals deviceGroup.Id
            orderby device.Name
            select new DeviceWithDetails(
                device.Id,
                device.Name,
                device.Description,
                device.InventoryNumber,
                deviceGroup,
                department
            )
        ).ToListAsync(cancellationToken);

    public Task<List<DeviceComponentWithDetails>> FindAllDeviceComponentsWithDetailsForDeviceAsync(
        DeviceId deviceId,
        CancellationToken cancellationToken = default
    ) =>
        (
            from device in _dbContext.Set<Device>()
            where device.Id == deviceId
            let deviceComponents = device.Components
            from deviceComponent in deviceComponents
            join controlSystem in _dbContext.Set<ControlSystem>()
                on deviceComponent.ControlSystemId equals controlSystem.Id
            join operatingSystem in _dbContext.Set<OperatingSystem>()
                on deviceComponent.OperatingSystemId equals operatingSystem.Id
            join component in _dbContext.Set<Component>()
                on deviceComponent.ComponentId equals component.Id
            select new DeviceComponentWithDetails(
                deviceComponent.Id,
                component,
                deviceComponent.IpAddress,
                operatingSystem,
                controlSystem,
                deviceComponent.CreatedOnUtc,
                deviceComponent.ModifiedOnUtc
            )
        ).ToListAsync(cancellationToken);
}
