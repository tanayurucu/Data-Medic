using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Devices;

using ErrorOr;

namespace DataMedic.Application.Devices.Commands.CreateDevice;

public sealed record CreateDeviceCommand(
    string Name,
    string Description,
    string InventoryNumber,
    Guid DeviceGroupId,
    Guid DepartmentId
) : ICommand<ErrorOr<Device>>;