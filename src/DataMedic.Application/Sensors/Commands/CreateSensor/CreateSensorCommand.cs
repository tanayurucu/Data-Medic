using DataMedic.Application.Common.Messages;
using DataMedic.Application.Sensors.Models;

using ErrorOr;

namespace DataMedic.Application.Sensors.Commands.CreateSensor;

public sealed record CreateSensorCommand(
    int SensorType,
    Guid HostId,
    Guid DeviceComponentId,
    Guid DeviceId,
    string Description,
    ICreateSensorDetailCommand SensorDetail
) : ICommand<ErrorOr<SensorWithDetails>>;

public interface ICreateSensorDetailCommand { }

public record CreateDockerSensorDetailCommand(
    int PortainerId,
    string ContainerId,
    int ScanPeriodInMinutes
) : ICreateSensorDetailCommand;

public record CreatePingSensorDetailCommand(int ScanPeriodInMinutes) : ICreateSensorDetailCommand;

public record CreateKafkaSensorDetailCommand(
    string TopicName,
    int TimeToLiveInSeconds,
    string IdentifierKey,
    string IdentifierValue
) : ICreateSensorDetailCommand;

public record CreateMqttSensorDetailCommand(string TopicName, int TimeToLiveInSeconds)
    : ICreateSensorDetailCommand;

public record CreateNodeRedSensorDetailCommand(string FlowId, int ScanPeriodInMinutes)
    : ICreateSensorDetailCommand;
