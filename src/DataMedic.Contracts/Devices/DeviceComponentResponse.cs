namespace DataMedic.Contracts.Devices;

public record DeviceComponentResponse(
    Guid ComponentId,
    string IpAddress,
    Guid Id,
    Guid OperatingSystemId,
    Guid ControlSystemId
);
