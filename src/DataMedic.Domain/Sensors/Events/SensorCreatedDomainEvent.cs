using DataMedic.Domain.Common.Events;

using MediatR;

namespace DataMedic.Domain.Sensors.Events;

public sealed record SensorCreatedDomainEvent(
    Guid Id,
    int Type,
    Guid DetailId,
    Guid SenosorId,
    Guid HostId,
    Guid DeviceComponentId
) : DomainEvent(Id);