using DataMedic.Application.Common.Messages;
using DataMedic.Domain.DeviceGroups;

using ErrorOr;

namespace DataMedic.Application.DeviceGroups.Commands.CreateDeviceGroup;

public sealed record CreateDeviceGroupCommand(string Name) : ICommand<ErrorOr<DeviceGroup>>;