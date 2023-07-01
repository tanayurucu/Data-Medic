using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Application.Sensors.Models;
using DataMedic.Domain.Devices.ValueObjects;
using DataMedic.Domain.Sensors;

using ErrorOr;

namespace DataMedic.Application.Sensors.Queries.GetAllSensors;

public sealed class GetAllSensorsQueryHandler
    : IQueryHandler<GetAllSensorsQuery, ErrorOr<List<SensorWithSensorDetail>>>
{
    private readonly ISensorRepository _sensorRepository;

    public GetAllSensorsQueryHandler(ISensorRepository sensorRepository)
    {
        _sensorRepository = sensorRepository;
    }

    public async Task<ErrorOr<List<SensorWithSensorDetail>>> Handle(
        GetAllSensorsQuery request,
        CancellationToken cancellationToken
    )
    {
        var deviceComponentId = DeviceComponentId.Create(request.DeviceComponentId);
        return await _sensorRepository.FindAllSensorsByDeviceComponentIdAsync(
            deviceComponentId,
            cancellationToken
        );
    }
}
