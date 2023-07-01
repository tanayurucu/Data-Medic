using DataMedic.Application.Common.Messages;
using DataMedic.Application.Sensors.Models;

using ErrorOr;

namespace DataMedic.Application.Sensors.Queries.GetSensorById;

public sealed record GetSensorByIdQuery(Guid SensorId) : IQuery<ErrorOr<SensorWithSensorDetail>>;
