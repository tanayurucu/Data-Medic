using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Application.Common.Interfaces.Infrastructure;

using ErrorOr;
using IServiceProvider = DataMedic.Application.Common.Interfaces.Infrastructure.IServiceProvider;
using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Domain.Sensors.ValueObjects;
using DataMedic.Domain.Sensors;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Hosts.ValueObjects;
using DataMedic.Domain.Hosts;
using DataMedic.Domain.Sensors.Entities;

namespace DataMedic.Application.Sensors.Commands.UpdateSensor;

public sealed class UpdateSensorCommandHandler
    : ICommandHandler<UpdateSensorCommand, ErrorOr<Updated>>
{
    private readonly ISensorRepository _sensorRepository;

    private readonly IHostRepository _hostRepository;
    private readonly IServiceProvider _serviceProvider;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSensorCommandHandler(
        ISensorRepository sensorRepository,
        IHostRepository hostRepository,
        IServiceProvider serviceProvider,
        IUnitOfWork unitOfWork
    )
    {
        _sensorRepository = sensorRepository;
        _hostRepository = hostRepository;
        _serviceProvider = serviceProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Updated>> Handle(
        UpdateSensorCommand request,
        CancellationToken cancellationToken
    )
    {
        var sensorId = SensorId.Create(request.SensorId);
        if (await _sensorRepository.FindByIdAsync(sensorId, cancellationToken) is not Sensor sensor)
        {
            return Errors.Sensor.NotFound(sensorId);
        }

        var hostId = HostId.Create(request.HostId);
        if (await _hostRepository.FindByIdAsync(hostId, cancellationToken) is not Host host)
        {
            return Errors.Host.NotFoundWithHostId(hostId);
        }

        ErrorOr<Updated> updateSensorResult = request.SensorDetail switch
        {
            UpdateDockerSensorDetailCommand updateDockerSensorDetailCommand
                when sensor.SensorDetail.Type == SensorType.DOCKER && host.Type == HostType.DOCKER
                => await UpdateDockerSensorAsync(
                    sensor,
                    updateDockerSensorDetailCommand,
                    cancellationToken
                ),
            UpdateKafkaSensorDetailCommand updateKafkaSensorDetailCommand
                when sensor.SensorDetail.Type == SensorType.KAFKA && host.Type == HostType.KAFKA
                => await UpdateKafkaSensorAsync(
                    sensor,
                    updateKafkaSensorDetailCommand,
                    cancellationToken
                ),
            UpdateMqttSensorDetailCommand updateMqttSensorDetailCommand
                when sensor.SensorDetail.Type == SensorType.MQTT && host.Type == HostType.MQTT
                => await UpdateMqttSensorAsync(
                    sensor,
                    updateMqttSensorDetailCommand,
                    cancellationToken
                ),
            UpdateNodeRedSensorDetailCommand updateNodeRedSensorDetailCommand
                when sensor.SensorDetail.Type == SensorType.NODE_RED
                    && host.Type == HostType.NODE_RED
                => await UpdateNodeRedSensorAsync(
                    sensor,
                    updateNodeRedSensorDetailCommand,
                    cancellationToken
                ),
            UpdatePingSensorDetailCommand updatePingSensorDetailCommand
                when sensor.SensorDetail.Type == SensorType.PING && host.Type == HostType.PING
                => await UpdatePingSensorAsync(
                    sensor,
                    updatePingSensorDetailCommand,
                    cancellationToken
                ),
            _ => Error.Conflict("Sensor.Update.Invalid", "Invalid update request.")
        };

        if (updateSensorResult.IsError)
        {
            return updateSensorResult.Errors;
        }

        sensor.SetDescription(request.Description);
        sensor.SetIsActive(request.IsActive);
        var updateHostResult = sensor.SetHost(host);
        if (updateHostResult.IsError)
        {
            return updateHostResult.Errors;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Updated;
    }

    private async Task<ErrorOr<Updated>> UpdatePingSensorAsync(
        Sensor sensor,
        UpdatePingSensorDetailCommand command,
        CancellationToken cancellationToken
    )
    {
        if (
            await _sensorRepository.FindPingSensorDetailByIdAsync(
                PingSensorId.Create(sensor.SensorDetail.DetailId),
                cancellationToken
            )
            is not PingSensor pingSensor
        )
        {
            return Error.NotFound(
                "Sensor.Ping.NotFound",
                $"Ping sensor with ID '{sensor.SensorDetail.DetailId}' not found."
            );
        }

        pingSensor.SetScanPeriod(TimeSpan.FromMinutes(command.ScanPeriodInMinutes));

        return Result.Updated;
    }

    private async Task<ErrorOr<Updated>> UpdateNodeRedSensorAsync(
        Sensor sensor,
        UpdateNodeRedSensorDetailCommand command,
        CancellationToken cancellationToken
    )
    {
        if (
            await _sensorRepository.FindNodeRedSensorDetailByIdAsync(
                NodeRedSensorId.Create(sensor.SensorDetail.DetailId),
                cancellationToken
            )
            is not NodeRedSensor nodeRedSensor
        )
        {
            return Error.NotFound(
                "Sensor.NodeRed.NotFound",
                $"NodeRed sensor with ID '{sensor.SensorDetail.DetailId}' not found."
            );
        }

        // TODO: validate flow exists

        nodeRedSensor.SetFlowId(command.FlowId);
        nodeRedSensor.SetScanPeriodInMinutes(TimeSpan.FromMinutes(command.ScanPeriodInMinutes));

        return Result.Updated;
    }

    private async Task<ErrorOr<Updated>> UpdateMqttSensorAsync(
        Sensor sensor,
        UpdateMqttSensorDetailCommand command,
        CancellationToken cancellationToken
    )
    {
        if (
            await _sensorRepository.FindMqttSensorDetailByIdAsync(
                MqttSensorId.Create(sensor.SensorDetail.DetailId),
                cancellationToken
            )
            is not MqttSensor mqttSensor
        )
        {
            return Error.NotFound(
                "Sensor.Mqtt.NotFound",
                $"Mqtt sensor with ID '{sensor.SensorDetail.DetailId}' not found."
            );
        }

        var createTopicNameResult = TopicName.Create(command.TopicName);
        if (createTopicNameResult.IsError)
        {
            return createTopicNameResult.Errors;
        }

        mqttSensor.SetTopicName(createTopicNameResult.Value);
        mqttSensor.SetTimeToLiveInSeconds(TimeSpan.FromSeconds(command.TimeToLiveInSeconds));

        return Result.Updated;
    }

    private async Task<ErrorOr<Updated>> UpdateKafkaSensorAsync(
        Sensor sensor,
        UpdateKafkaSensorDetailCommand command,
        CancellationToken cancellationToken = default
    )
    {
        if (
            await _sensorRepository.FindKafkaSensorDetailByIdAsync(
                KafkaSensorId.Create(sensor.SensorDetail.DetailId),
                cancellationToken
            )
            is not KafkaSensor kafkaSensor
        )
        {
            return Error.NotFound(
                "Sensor.Kafka.NotFound",
                $"Kafka sensor with ID '{sensor.SensorDetail.DetailId}' not found."
            );
        }

        //TODO: validate topic exists
        var createKafkaTopicResult = TopicName.Create(command.TopicName);
        if (createKafkaTopicResult.IsError)
        {
            return createKafkaTopicResult.Errors;
        }
        kafkaSensor.SetIdentifierKey(command.IdentifierKey);
        kafkaSensor.SetIdentifierValue(command.IdentifierValue);
        kafkaSensor.SetTopicName(createKafkaTopicResult.Value);
        kafkaSensor.SetTimeToLiveInSeconds(TimeSpan.FromSeconds(command.TimeToLiveInSeconds));
        return Result.Updated;
    }

    private async Task<ErrorOr<Updated>> UpdateDockerSensorAsync(
        Sensor sensor,
        UpdateDockerSensorDetailCommand command,
        CancellationToken cancellationToken = default
    )
    {
        if (
            await _sensorRepository.FindDockerSensorDetailByIdAsync(
                DockerSensorId.Create(sensor.SensorDetail.DetailId),
                cancellationToken
            )
            is not DockerSensor dockerSensor
        )
        {
            return Error.NotFound(
                "Sensor.Docker.NotFound",
                $"Docker sensor detail with ID '{sensor.SensorDetail.DetailId}' not found."
            );
        }
        // TODO: validate container exists
        
        dockerSensor.SetContainerId(command.ContainerId);
        dockerSensor.SetPortainerId(command.PortainerId);
        dockerSensor.SetScanPeriod(TimeSpan.FromMinutes(command.ScanPeriodInMinutes));

        return Result.Updated;
    }
}
