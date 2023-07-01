namespace DataMedic.Contracts.Devices;

public record CreateDeviceRequest(
    string Name,
    string Description,
    string InventoryNumber,
    Guid DeviceGroupId,
    Guid DepartmentId
);
