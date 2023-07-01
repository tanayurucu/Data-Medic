using DataMedic.Domain.Common.Abstractions;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Common.Interfaces;
using DataMedic.Domain.Devices.ValueObjects;
using DataMedic.Domain.Hosts;
using DataMedic.Domain.Hosts.ValueObjects;
using DataMedic.Domain.Sensors.Events;
using DataMedic.Domain.Sensors.ValueObjects;

using ErrorOr;

namespace DataMedic.Domain.Sensors;

public sealed class Sensor : AggregateRoot<SensorId>, IAuditableEntity, ISoftDeletableEntity
{
    public SensorDetail SensorDetail { get; private set; }
    public DeviceComponentId DeviceComponentId { get; private set; }
    public HostId HostId { get; private set; }
    public bool Status { get; private set; }
    public string StatusText { get; private set; }
    public string Description { get; private set; }
    public DateTime? LastCheckOnUtc { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? ModifiedOnUtc { get; private set; }
    public DateTime? DeletedOnUtc { get; private set; }
    public bool IsDeleted { get; private set; }

    private Sensor() { }

    private Sensor(
        SensorId id,
        DeviceComponentId deviceComponentId,
        bool status,
        string description,
        string statusText,
        HostId hostId,
        SensorDetail sensorDetail
    )
        : base(id)
    {
        DeviceComponentId = deviceComponentId;
        Status = status;
        Description = description;
        StatusText = statusText;
        HostId = hostId;
        AddDomainEvent(new SensorCreatedDomainEvent(Guid.NewGuid(),((int)sensorDetail.Type),sensorDetail.DetailId, Id.Value,hostId.Value,deviceComponentId.Value));
        SensorDetail = sensorDetail;
        IsActive = true;
    }

    public static Sensor Create(
        DeviceComponentId deviceComponentId,
        HostId hostId,
        string description,
        SensorDetail sensorDetail
    ) =>
        new(
            SensorId.CreateUnique(),
            deviceComponentId,
            false,
            description,
            string.Empty,
            hostId,
            sensorDetail
        );

    public void SetDescription(string description) => Description = description;

    public void SetIsActive(bool isActive) => IsActive = isActive;

    public ErrorOr<Updated> SetHost(Host host)
    {
        if ((int)host.Type != (int)SensorDetail.Type)
        {
            return Errors.Host.HostAndSensorTypeNotMatch;
        }

        HostId = host.Id;

        return Result.Updated;
    }

    public void Delete(Guid sensorId, int type)
    {
        AddDomainEvent(
            new SensorDeletedDomainEvent(Guid.NewGuid(),sensorId, type)
        );
    }
    public void SetStatus(bool status)
    {
        (Status, LastCheckOnUtc) = (status, DateTime.UtcNow);
        AddDomainEvent(
            new SensorStatusUpdatedDomainEvent(Guid.NewGuid(),Id.Value,Status,(int)SensorDetail.Type)
            );
    }

    public void SetStatusText(string? text) => (StatusText,LastCheckOnUtc) = (text,DateTime.UtcNow);
    public void SetLastCheck() => LastCheckOnUtc = DateTime.Now;

}
