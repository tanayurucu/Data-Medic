using DataMedic.Application.Common.Messages;

using ErrorOr;

namespace DataMedic.Application.DeviceGroups.Commands.UpdateDeviceGroup;

public sealed record UpdateDeviceGroupCommand(Guid DeviceGroupId, string Name) : ICommand<ErrorOr<Updated>>;