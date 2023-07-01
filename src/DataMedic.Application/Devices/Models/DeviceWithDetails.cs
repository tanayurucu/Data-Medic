using DataMedic.Domain.Departments;
using DataMedic.Domain.DeviceGroups;
using DataMedic.Domain.Devices.ValueObjects;

namespace DataMedic.Application.Devices.Models;

public sealed record DeviceWithDetails(
    DeviceId Id,
    DeviceName Name,
    string Description,
    InventoryNumber InventoryNumber,
    DeviceGroup DeviceGroup,
    Department Department
);
