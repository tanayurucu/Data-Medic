using DataMedic.Application.Common.Messages;

using ErrorOr;

namespace DataMedic.Application.DeviceGroups.Commands.DeleteDeviceGroup;

public sealed record DeleteDeviceGroupCommand(Guid DeviceGroupId) : ICommand<ErrorOr<Deleted>>;