using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Application.Devices.Models;
using DataMedic.Domain.Devices.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.Devices.Queries.GetAllDeviceComponents;

public sealed class GetAllDeviceComponentsQueryHandler
    : IQueryHandler<GetAllDeviceComponentsQuery, ErrorOr<List<DeviceComponentWithDetails>>>
{
    private readonly IDeviceRepository _deviceRepository;

    public GetAllDeviceComponentsQueryHandler(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    public async Task<ErrorOr<List<DeviceComponentWithDetails>>> Handle(
        GetAllDeviceComponentsQuery request,
        CancellationToken cancellationToken
    )
    {
        var deviceId = DeviceId.Create(request.DeviceId);

        return await _deviceRepository.FindAllDeviceComponentsWithDetailsForDeviceAsync(
            deviceId,
            cancellationToken
        );
    }
}
