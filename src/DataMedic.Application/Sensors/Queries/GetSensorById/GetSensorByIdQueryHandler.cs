using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Application.Sensors.Models;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Sensors;
using DataMedic.Domain.Sensors.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.Sensors.Queries.GetSensorById;

public sealed class GetSensorByIdQueryHandler
    : IQueryHandler<GetSensorByIdQuery, ErrorOr<SensorWithSensorDetail>>
{
    private readonly ISensorRepository _sensorRepository;

    public GetSensorByIdQueryHandler(ISensorRepository sensorRepository)
    {
        _sensorRepository = sensorRepository;
    }

    public async Task<ErrorOr<SensorWithSensorDetail>> Handle(
        GetSensorByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var sensorId = SensorId.Create(request.SensorId);
        if (await _sensorRepository.FindByIdAsync(sensorId, cancellationToken) is not Sensor sensor)
        {
            return Errors.Sensor.NotFound(sensorId);
        }
        if (
            await _sensorRepository.FindSensorDetailByIdAsync(
                sensor.SensorDetail,
                cancellationToken
            )
            is not ISensorDetail sensorDetail
        )
        {
            return Errors.Sensor.NotFound(sensorId);
        }
        return new SensorWithSensorDetail(
            sensor.Id,
            sensor.SensorDetail.Type,
            sensor.Description,
            sensor.Status,
            sensor.StatusText,
            sensor.IsActive,
            sensor.HostId,
            sensor.LastCheckOnUtc,
            sensor.CreatedOnUtc,
            sensor.ModifiedOnUtc,
            sensorDetail
        );
    }
}
