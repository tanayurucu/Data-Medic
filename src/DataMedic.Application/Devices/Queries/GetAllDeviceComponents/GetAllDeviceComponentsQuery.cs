using DataMedic.Application.Common.Messages;
using DataMedic.Application.Devices.Models;

using ErrorOr;

namespace DataMedic.Application.Devices.Queries.GetAllDeviceComponents;

public sealed record GetAllDeviceComponentsQuery(Guid DeviceId)
    : IQuery<ErrorOr<List<DeviceComponentWithDetails>>>;
