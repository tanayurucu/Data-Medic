using DataMedic.Application.Common.Messages;
using DataMedic.Domain.DeviceGroups;

using ErrorOr;

namespace DataMedic.Application.DeviceGroups.Queries.GetAllDeviceGroups;

public sealed record GetAllDeviceGroupsQuery() : IQuery<ErrorOr<List<DeviceGroup>>>;