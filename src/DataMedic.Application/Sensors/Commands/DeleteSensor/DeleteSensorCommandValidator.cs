using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

using FluentValidation;

namespace DataMedic.Application.Sensors.Commands.DeleteSensor;

public class DeleteSensorCommandValidator : AbstractValidator<DeleteSensorCommand>
{
    public DeleteSensorCommandValidator()
    {
        RuleFor(x => x.SensorId)
            .NotEmpty()
            .WithError(Errors.Sensor.Id.Empty)
            .NotEqual(Guid.Empty)
            .WithError(Errors.Sensor.Id.Empty);
    }
}