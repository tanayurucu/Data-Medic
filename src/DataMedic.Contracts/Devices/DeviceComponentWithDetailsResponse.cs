using DataMedic.Contracts.Components;
using DataMedic.Contracts.ControlSystems;
using DataMedic.Contracts.OperatingSystems;

namespace DataMedic.Contracts.Devices;

public sealed record DeviceComponentWithDetailsResponse(
    Guid Id,
    ComponentResponse Component,
    string IpAddress,
    OperatingSystemResponse OperatingSystem,
    ControlSystemResponse ControlSystem,
    DateTime CreatedOnUtc,
    DateTime? ModifiedOnUtc
);
