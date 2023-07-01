using DataMedic.Application.Common.Messages;
using DataMedic.Domain.DeviceGroups;

using ErrorOr;

namespace DataMedic.Application.DeviceGroups.Queries.GetDeviceGroupById;

public sealed record GetDeviceGroupByIdQuery(Guid DeviceGroupId) : IQuery<ErrorOr<DeviceGroup>>;