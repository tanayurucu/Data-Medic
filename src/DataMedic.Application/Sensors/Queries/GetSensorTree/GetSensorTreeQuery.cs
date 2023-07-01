using DataMedic.Application.Common.Messages;
using DataMedic.Application.Sensors.Models;

using ErrorOr;

namespace DataMedic.Application.Sensors.Queries.GetSensorTree;

public sealed record GetSensorTreeQuery(
    bool ShowOnlyUpSensors,
    bool ShowOnlyDownSensors,
    bool ShowOnlyInactiveSensors,
    string SearchString,
    Guid ControlSystemId,
    Guid OperatingSystemId,
    Guid DeviceGroupId,
    Guid DepartmentId
) : IQuery<ErrorOr<SensorTree>>;
