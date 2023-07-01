using DataMedic.Application.Common.Messages;
using DataMedic.Application.Sensors.Models;
using DataMedic.Domain.Sensors;

using ErrorOr;

namespace DataMedic.Application.Sensors.Queries.GetAllSensors;

public sealed record GetAllSensorsQuery(
    Guid DeviceComponentId) : IQuery<ErrorOr<List<SensorWithSensorDetail>>>;