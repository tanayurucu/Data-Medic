using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Devices;
using DataMedic.Domain.Devices.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.Devices.Queries.GetDeviceById;

internal sealed class GetDeviceByIdQueryHandler : IQueryHandler<GetDeviceByIdQuery, ErrorOr<Device>>
{
    private readonly IDeviceRepository _deviceRepository;

    public GetDeviceByIdQueryHandler(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    public async Task<ErrorOr<Device>> Handle(GetDeviceByIdQuery request, CancellationToken cancellationToken)
    {
        var deviceId = DeviceId.Create(request.DeviceId);

        if (await _deviceRepository.FindByIdAsync(deviceId, cancellationToken) is not Device device)
        {
            return Errors.Device.NotFound;
        }

        return device;
    }
}