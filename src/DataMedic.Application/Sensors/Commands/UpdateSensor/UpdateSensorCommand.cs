using DataMedic.Application.Common.Messages;

using ErrorOr;

namespace DataMedic.Application.Sensors.Commands.UpdateSensor;

public sealed record UpdateSensorCommand(
    Guid SensorId,
    Guid HostId,
    bool IsActive,
    string Description,
    IUpdateSensorDetailCommand SensorDetail
) : ICommand<ErrorOr<Updated>>;

public interface IUpdateSensorDetailCommand { }

public record UpdateDockerSensorDetailCommand(
    int PortainerId,
    string ContainerId,
    int ScanPeriodInMinutes
) : IUpdateSensorDetailCommand;

public record UpdatePingSensorDetailCommand(int ScanPeriodInMinutes) : IUpdateSensorDetailCommand;

public record UpdateKafkaSensorDetailCommand(
    string TopicName,
    int TimeToLiveInSeconds,
    string IdentifierKey,
    string IdentifierValue
) : IUpdateSensorDetailCommand;

public record UpdateMqttSensorDetailCommand(string TopicName, int TimeToLiveInSeconds)
    : IUpdateSensorDetailCommand;

public record UpdateNodeRedSensorDetailCommand(string FlowId, int ScanPeriodInMinutes)
    : IUpdateSensorDetailCommand;
