using DataMedic.Domain.Components;
using DataMedic.Domain.ControlSystems;
using DataMedic.Domain.Devices.ValueObjects;

using OperatingSystem = DataMedic.Domain.OperatingSystems.OperatingSystem;

namespace DataMedic.Application.Devices.Models;

public sealed record DeviceComponentWithDetails(
    DeviceComponentId Id,
    Component Component,
    IpAddress IpAddress,
    OperatingSystem OperatingSystem,
    ControlSystem ControlSystem,
    DateTime CreatedOnUtc,
    DateTime? ModifiedOnUtc
);
