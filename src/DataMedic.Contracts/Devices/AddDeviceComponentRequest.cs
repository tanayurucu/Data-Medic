namespace DataMedic.Contracts.Devices;

public record AddDeviceComponentRequest(
    Guid ComponentId,
    string IpAddress,
    Guid OperatingSystemId,
    Guid ControlSystemId
);
