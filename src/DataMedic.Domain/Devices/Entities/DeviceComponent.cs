using DataMedic.Domain.Common.Abstractions;
using DataMedic.Domain.Common.Interfaces;
using DataMedic.Domain.Components;
using DataMedic.Domain.Components.ValueObjects;
using DataMedic.Domain.ControlSystems.ValueObjects;
using DataMedic.Domain.Devices.ValueObjects;
using DataMedic.Domain.OperatingSystems.ValueObjects;
using DataMedic.Domain.ControlSystems;

using OperatingSystem = DataMedic.Domain.OperatingSystems.OperatingSystem;

namespace DataMedic.Domain.Devices.Entities;

public sealed class DeviceComponent : Entity<DeviceComponentId>, IAuditableEntity
{
    public ComponentId ComponentId { get; private set; }
    public IpAddress IpAddress { get; private set; }
    public OperatingSystemId OperatingSystemId { get; private set; }
    public ControlSystemId ControlSystemId { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? ModifiedOnUtc { get; private set; }

    public DeviceComponent(
        DeviceComponentId id,
        ComponentId componentId,
        IpAddress ipAddress,
        OperatingSystemId operatingSystemId,
        ControlSystemId controlSystemId
    )
        : base(id)
    {
        ComponentId = componentId;
        IpAddress = ipAddress;
        OperatingSystemId = operatingSystemId;
        ControlSystemId = controlSystemId;
    }

    private DeviceComponent() { }

    public static DeviceComponent Create(
        Component component,
        IpAddress ipAddress,
        OperatingSystem operatingSystem,
        ControlSystem controlSystem
    ) =>
        new(
            DeviceComponentId.CreateUnique(),
            component.Id,
            ipAddress,
            operatingSystem.Id,
            controlSystem.Id
        );

    public void SetComponent(Component component) => ComponentId = component.Id;

    public void SetIpAddress(IpAddress ipAddress) => IpAddress = ipAddress;

    public void SetOperatingSystem(OperatingSystem operatingSystem) =>
        OperatingSystemId = operatingSystem.Id;

    public void SetControlSystem(ControlSystem controlSystem) => ControlSystemId = controlSystem.Id;
}
