using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Sensors;
using DataMedic.Domain.Sensors.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.Sensors.Commands.DeleteSensor;

public sealed class DeleteSensorCommandHandler
    : ICommandHandler<DeleteSensorCommand, ErrorOr<Deleted>>
{
    private readonly ISensorRepository _sensorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSensorCommandHandler(ISensorRepository sensorRepository, IUnitOfWork unitOfWork)
    {
        _sensorRepository = sensorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Deleted>> Handle(
        DeleteSensorCommand request,
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

        _sensorRepository.RemoveSensorDetail(sensorDetail);
        _sensorRepository.Remove(sensor);
        sensor.Delete(request.SensorId, (int)sensor.SensorDetail.Type);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Deleted;
    }
}
