using DataMedic.Application.Common.Messages;

using ErrorOr;

namespace DataMedic.Application.Devices.Commands.UpdateDeviceComponent;

public sealed record UpdateDeviceComponentCommand(
    Guid DeviceId,
    Guid DeviceComponentId,
    string DeviceComponentName,
    string IpAddress,
    Guid OperatingSystemId,
    Guid ControlSystemId
) : ICommand<ErrorOr<Updated>>;
