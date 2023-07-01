namespace DataMedic.Contracts.Devices;

public record UpdateDeviceRequest(
    string Name,
    string Description,
    string InventoryNumber,
    Guid DeviceGroupId,
    Guid DepartmentId
);
