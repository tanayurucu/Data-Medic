using DataMedic.Application.Common.Messages;

using ErrorOr;

namespace DataMedic.Application.Devices.Commands.DeleteDeviceComponent;

public sealed record DeleteDeviceComponentCommand(Guid DeviceId, Guid DeviceComponentId)
    : ICommand<ErrorOr<Deleted>>;