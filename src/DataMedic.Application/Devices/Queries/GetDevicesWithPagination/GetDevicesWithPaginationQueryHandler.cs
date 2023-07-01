using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Application.Common.Models;
using DataMedic.Application.Devices.Models;
using DataMedic.Domain.Departments.ValueObjects;
using DataMedic.Domain.DeviceGroups.ValueObjects;
using DataMedic.Domain.Devices;

using ErrorOr;

namespace DataMedic.Application.Devices.Queries.GetDevicesWithPagination;

internal sealed class GetDevicesWithPaginationQueryHandler
    : IQueryHandler<GetDevicesWithPaginationQuery, ErrorOr<Paged<DeviceWithDetails>>>
{
    private readonly IDeviceRepository _deviceRepository;

    public GetDevicesWithPaginationQueryHandler(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    public async Task<ErrorOr<Paged<DeviceWithDetails>>> Handle(
        GetDevicesWithPaginationQuery request,
        CancellationToken cancellationToken
    )
    {
        var departmentId = DepartmentId.Create(request.DepartmentId);
        var deviceGroupId = DeviceGroupId.Create(request.DeviceGroupId);
        return await _deviceRepository.FindManyWithPaginationAsync(
            request.SearchString,
            request.PageSize,
            request.PageNumber,
            departmentId,
            deviceGroupId,
            cancellationToken
        );
    }
}