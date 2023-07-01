namespace DataMedic.Contracts.Devices;

public record DeviceResponse(
    Guid Id,
    string Name,
    string Description,
    string InventoryNumber,
    Guid DeviceGroupId,
    Guid DepartmentId
);
