using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.DeviceGroups;

using ErrorOr;

namespace DataMedic.Application.DeviceGroups.Queries.GetAllDeviceGroups;

internal sealed class GetAllDeviceGroupsQueryHandler : IQueryHandler<GetAllDeviceGroupsQuery, ErrorOr<List<DeviceGroup>>>
{
    private readonly IDeviceGroupRepository _deviceGroupRepository;

    public GetAllDeviceGroupsQueryHandler(IDeviceGroupRepository deviceGroupRepository)
    {
        _deviceGroupRepository = deviceGroupRepository;
    }

    public async Task<ErrorOr<List<DeviceGroup>>> Handle(GetAllDeviceGroupsQuery request, CancellationToken cancellationToken)
    {
        return await _deviceGroupRepository.FindAllAsync(cancellationToken);
    }
}