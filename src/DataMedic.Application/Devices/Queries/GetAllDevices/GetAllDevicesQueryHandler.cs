using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Application.Devices.Models;

using ErrorOr;

namespace DataMedic.Application.Devices.Queries.GetAllDevices;

public sealed class GetAllDevicesQueryHandler
    : IQueryHandler<GetAllDevicesQuery, ErrorOr<List<DeviceWithDetails>>>
{
    private readonly IDeviceRepository _deviceRepository;

    public GetAllDevicesQueryHandler(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    public async Task<ErrorOr<List<DeviceWithDetails>>> Handle(
        GetAllDevicesQuery request,
        CancellationToken cancellationToken
    )
    {
        return await _deviceRepository.FindAllWithDetailsAsync(cancellationToken);
    }
}
