using DataMedic.Contracts.Departments;
using DataMedic.Contracts.DeviceGroups;

namespace DataMedic.Contracts.Devices;

public record DeviceWithDetailsResponse(
    Guid Id,
    string Name,
    string Description,
    string InventoryNumber,
    DeviceGroupResponse DeviceGroup,
    DepartmentResponse Department
);
