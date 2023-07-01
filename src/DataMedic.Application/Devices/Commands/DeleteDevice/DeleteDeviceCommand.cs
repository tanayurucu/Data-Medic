using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Devices.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.Devices.Commands.DeleteDevice;

public sealed record DeleteDeviceCommand(Guid DeviceId) : ICommand<ErrorOr<Deleted>>;