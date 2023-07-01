using DataMedic.Application.Common.Messages;
using DataMedic.Application.Common.Models;
using DataMedic.Application.Devices.Models;
using DataMedic.Domain.Devices.Entities;

using ErrorOr;

namespace DataMedic.Application.Devices.Queries.GetDeviceComponentsWithPagination;

public record GetDeviceComponentsWithPaginationQuery(
    string SearchString,
    int PageSize,
    int PageNumber,
    Guid DeviceId,
    Guid OperatingSystemId,
    Guid ControlSystemId
) : IQuery<ErrorOr<Paged<DeviceComponentWithDetails>>>;
