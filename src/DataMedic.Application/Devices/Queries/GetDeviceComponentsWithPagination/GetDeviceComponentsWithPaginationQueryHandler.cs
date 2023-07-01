using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Application.Common.Models;
using DataMedic.Application.Devices.Models;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.ControlSystems.ValueObjects;
using DataMedic.Domain.Devices;
using DataMedic.Domain.Devices.Entities;
using DataMedic.Domain.Devices.ValueObjects;
using DataMedic.Domain.OperatingSystems.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.Devices.Queries.GetDeviceComponentsWithPagination;

internal sealed class GetDeviceComponentsWithPaginationQueryHandler
    : IQueryHandler<
        GetDeviceComponentsWithPaginationQuery,
        ErrorOr<Paged<DeviceComponentWithDetails>>
    >
{
    private readonly IDeviceRepository _deviceRepository;

    public GetDeviceComponentsWithPaginationQueryHandler(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    public async Task<ErrorOr<Paged<DeviceComponentWithDetails>>> Handle(
        GetDeviceComponentsWithPaginationQuery request,
        CancellationToken cancellationToken
    )
    {
        var deviceId = DeviceId.Create(request.DeviceId);
        if (await _deviceRepository.FindByIdAsync(deviceId, cancellationToken) is null)
        {
            return Errors.Device.NotFound;
        }

        var operatingSystemId = OperatingSystemId.Create(request.OperatingSystemId);
        var controlSystemId = ControlSystemId.Create(request.ControlSystemId);
        return await _deviceRepository.FindDeviceComponentsWithPaginationAsync(
            request.SearchString,
            request.PageSize,
            request.PageNumber,
            deviceId,
            operatingSystemId,
            controlSystemId,
            cancellationToken
        );
    }
}
