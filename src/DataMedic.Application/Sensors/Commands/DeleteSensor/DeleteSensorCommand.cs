using DataMedic.Application.Common.Messages;

using ErrorOr;

namespace DataMedic.Application.Sensors.Commands.DeleteSensor;

public sealed record DeleteSensorCommand(Guid SensorId) : ICommand<ErrorOr<Deleted>>;