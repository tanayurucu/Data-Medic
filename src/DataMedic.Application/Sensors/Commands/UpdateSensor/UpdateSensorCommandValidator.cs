using FluentValidation;

namespace DataMedic.Application.Sensors.Commands.UpdateSensor;

public sealed class UpdateSensorCommandValidator : AbstractValidator<UpdateSensorCommand>
{
    public UpdateSensorCommandValidator() { }
}
