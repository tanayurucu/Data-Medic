using DataMedic.Domain.Hosts.ValueObjects;
using DataMedic.Domain.Sensors;
using DataMedic.Domain.Sensors.ValueObjects;

namespace DataMedic.Application.Sensors.Models;

public sealed record SensorWithSensorDetail(
    SensorId Id,
    SensorType Type,
    string Description,
    bool Status,
    string StatusText,
    bool IsActive,
    HostId HostId,
    DateTime? LastCheckOnUtc,
    DateTime CreatedOnUtc,
    DateTime? ModifiedOnUtc,
    ISensorDetail SensorDetail
);
