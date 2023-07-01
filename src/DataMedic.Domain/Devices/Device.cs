using DataMedic.Domain.Common.Abstractions;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Common.Interfaces;
using DataMedic.Domain.Departments;
using DataMedic.Domain.Departments.ValueObjects;
using DataMedic.Domain.DeviceGroups;
using DataMedic.Domain.DeviceGroups.ValueObjects;
using DataMedic.Domain.Devices.Entities;
using DataMedic.Domain.Devices.ValueObjects;

using ErrorOr;

namespace DataMedic.Domain.Devices;

public sealed class Device : AggregateRoot<DeviceId>, IAuditableEntity, ISoftDeletableEntity
{
    private readonly List<DeviceComponent> _components = new();
    public IReadOnlyCollection<DeviceComponent> Components => _components.AsReadOnly();
    public DeviceName Name { get; private set; }
    public string Description { get; private set; }
    public InventoryNumber InventoryNumber { get; private set; }
    public DeviceGroupId DeviceGroupId { get; private set; }
    public DepartmentId DepartmentId { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? ModifiedOnUtc { get; private set; }
    public DateTime? DeletedOnUtc { get; private set; }
    public bool IsDeleted { get; private set; }

    private Device() { }

    private Device(
        DeviceId id,
        DeviceName name,
        string description,
        InventoryNumber inventoryNumber,
        DeviceGroupId deviceGroupId,
        DepartmentId departmentId
    )
        : base(id)
    {
        Name = name;
        Description = description;
        InventoryNumber = inventoryNumber;
        DeviceGroupId = deviceGroupId;
        DepartmentId = departmentId;
    }

    public static Device Create(
        DeviceName name,
        string description,
        InventoryNumber inventoryNumber,
        DeviceGroupId deviceGroupId,
        DepartmentId departmentId
    ) =>
        new(
            DeviceId.CreateUnique(),
            name,
            description,
            inventoryNumber,
            deviceGroupId,
            departmentId
        );

    public ErrorOr<DeviceComponent> AddDeviceComponent(DeviceComponent deviceComponent)
    {
        if (_components.Any(component => component.IpAddress == deviceComponent.IpAddress))
        {
            return Errors.Device.IpAddress.AlreadyExists(deviceComponent.IpAddress);
        }

        _components.Add(deviceComponent);
        return deviceComponent;
    }

    public ErrorOr<Deleted> RemoveDeviceComponentById(DeviceComponentId deviceComponentId)
    {
        if (
            _components.Find(component => component.Id == deviceComponentId)
            is not DeviceComponent deviceComponent
        )
        {
            return Errors.Device.DeviceComponent.NotFound;
        }
        _components.Remove(deviceComponent);
        return Result.Deleted;
    }

    public ErrorOr<DeviceComponent> FindDeviceComponentById(DeviceComponentId deviceComponentId)
    {
        if (
            _components.Find(component => component.Id == deviceComponentId)
            is not DeviceComponent deviceComponent
        )
        {
            return Errors.Device.DeviceComponent.NotFound;
        }
        return deviceComponent;
    }

    public void SetName(DeviceName name) => Name = name;

    public void SetDescription(string description) => Description = description;

    public void SetInventoryNumber(InventoryNumber inventoryNumber) =>
        InventoryNumber = inventoryNumber;

    public void SetDeviceGroup(DeviceGroupId deviceGroupId) => DeviceGroupId = deviceGroupId;

    public void SetDepartment(DepartmentId departmentId) => DepartmentId = departmentId;
}
