namespace DataMedic.Contracts.Devices;

public record UpdateDeviceComponentRequest(
    Guid ComponentId,
    string IpAddress,
    Guid OperatingSystemId,
    Guid ControlSystemId
);
