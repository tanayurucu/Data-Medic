using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Application.Common.Models;
using DataMedic.Domain.DeviceGroups;

using ErrorOr;

namespace DataMedic.Application.DeviceGroups.Queries.GetDeviceGroupsWithPagination;

internal sealed class GetDeviceGroupWithPaginationQueryHandler
    : IQueryHandler<GetDeviceGroupsWithPaginationQuery, ErrorOr<Paged<DeviceGroup>>>
{
    private readonly IDeviceGroupRepository _deviceGroupRepository;

    public GetDeviceGroupWithPaginationQueryHandler(IDeviceGroupRepository deviceGroupRepository)
    {
        _deviceGroupRepository = deviceGroupRepository;
    }

    public async Task<ErrorOr<Paged<DeviceGroup>>> Handle(
        GetDeviceGroupsWithPaginationQuery request,
        CancellationToken cancellationToken
    )
    {
        return await _deviceGroupRepository.FindManyWithPaginationAsync(
            request.SearchString,
            request.PageSize,
            request.PageNumber,
            cancellationToken
        );
    }
}