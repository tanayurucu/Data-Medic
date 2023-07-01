using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Devices.Entities;

using ErrorOr;

namespace DataMedic.Application.Devices.Commands.AddDeviceComponent;

public sealed record AddDeviceComponentCommand(
    Guid DeviceId,
    Guid ComponentId,
    string IpAddress,
    Guid OperatingSystemId,
    Guid ControlSystemId
) : ICommand<ErrorOr<DeviceComponent>>;