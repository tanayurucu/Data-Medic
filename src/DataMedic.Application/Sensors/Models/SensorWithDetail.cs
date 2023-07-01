using DataMedic.Domain.Devices;
using DataMedic.Domain.Devices.Entities;
using DataMedic.Domain.Devices.ValueObjects;
using DataMedic.Domain.Hosts;
using DataMedic.Domain.Hosts.ValueObjects;
using DataMedic.Domain.Sensors;
using DataMedic.Domain.Sensors.ValueObjects;

namespace DataMedic.Application.Sensors.Models;

public record SensorWithDetails(
    SensorId Id,
    string Description,
    bool Status,
    string StatusText,
    int Type,
    bool IsActive,
    Guid DetailId,
    DateTime? LastCheckOnUtc,
    DateTime CreatedOnUtc,
    DateTime? ModifiedOnUtc,
    ISensorDetail SensorDetail,
    Device Device,
    DeviceComponent DeviceComponent,
    Host Host
);
