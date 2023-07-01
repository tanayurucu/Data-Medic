using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Devices;

using ErrorOr;

namespace DataMedic.Application.Devices.Queries.GetDeviceById;

public sealed record GetDeviceByIdQuery(Guid DeviceId) : IQuery<ErrorOr<Device>>;