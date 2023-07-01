using DataMedic.Domain.Common.Events;

namespace DataMedic.Domain.Sensors.Events;


public sealed record SensorDeletedDomainEvent(
    Guid Id,
    Guid SenosorId,
    int Type
) : DomainEvent(Id);