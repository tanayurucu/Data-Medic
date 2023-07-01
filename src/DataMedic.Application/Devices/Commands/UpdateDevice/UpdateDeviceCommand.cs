using DataMedic.Application.Common.Messages;

using ErrorOr;

namespace DataMedic.Application.Devices.Commands.UpdateDevice;

public sealed record UpdateDeviceCommand(
    Guid DeviceId,
    string Name,
    string Description,
    string InventoryNumber,
    Guid DeviceGroupId,
    Guid DepartmentId
) : ICommand<ErrorOr<Updated>>;