using DataMedic.Application.Common.Messages;
using DataMedic.Application.Common.Models;
using DataMedic.Application.Devices.Models;
using DataMedic.Domain.Devices;

using ErrorOr;

namespace DataMedic.Application.Devices.Queries.GetDevicesWithPagination;

public sealed record GetDevicesWithPaginationQuery(
    string SearchString,
    int PageSize,
    int PageNumber,
    Guid DepartmentId,
    Guid DeviceGroupId
) : IQuery<ErrorOr<Paged<DeviceWithDetails>>>;