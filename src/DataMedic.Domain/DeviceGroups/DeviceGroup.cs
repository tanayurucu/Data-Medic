using DataMedic.Domain.Common.Abstractions;
using DataMedic.Domain.Common.Interfaces;
using DataMedic.Domain.DeviceGroups.ValueObjects;

namespace DataMedic.Domain.DeviceGroups;

public sealed class DeviceGroup
    : AggregateRoot<DeviceGroupId>,
        IAuditableEntity,
        ISoftDeletableEntity
{
    public DeviceGroupName Name { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public DateTime? ModifiedOnUtc { get; private set; }

    public DateTime? DeletedOnUtc { get; private set; }

    public bool IsDeleted { get; private set; }

    private DeviceGroup()
    {
    }
    private DeviceGroup(DeviceGroupId id, DeviceGroupName name)
        : base(id) => Name = name;

    public static DeviceGroup Create(DeviceGroupName name) =>
        new(DeviceGroupId.CreateUnique(), name);

    public void SetName(DeviceGroupName name) => Name = name;
}