using DataMedic.Application.Common.Messages;
using DataMedic.Application.Common.Models;
using DataMedic.Domain.DeviceGroups;

using ErrorOr;

namespace DataMedic.Application.DeviceGroups.Queries.GetDeviceGroupsWithPagination;

public record GetDeviceGroupsWithPaginationQuery(string SearchString, int PageSize, int PageNumber)
    : IQuery<ErrorOr<Paged<DeviceGroup>>>;