using DataMedic.Domain.Common.Events;

namespace DataMedic.Domain.Sensors.Events;

public sealed record SensorStatusUpdatedDomainEvent(
    Guid Id,
    Guid SenosorId,
    bool Status,
    int Type
) : DomainEvent(Id);