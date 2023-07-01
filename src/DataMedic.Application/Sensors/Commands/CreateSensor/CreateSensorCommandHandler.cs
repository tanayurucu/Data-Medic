using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Application.Sensors.Models;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Devices;
using DataMedic.Domain.Devices.ValueObjects;
using DataMedic.Domain.Hosts;
using DataMedic.Domain.Hosts.ValueObjects;
using DataMedic.Domain.Sensors;
using DataMedic.Domain.Sensors.Entities;
using DataMedic.Domain.Sensors.ValueObjects;

using ErrorOr;
using IServiceProvider = DataMedic.Application.Common.Interfaces.Infrastructure.IServiceProvider;

namespace DataMedic.Application.Sensors.Commands.CreateSensor;

public sealed class CreateSensorCommandHandler
    : ICommandHandler<CreateSensorCommand, ErrorOr<SensorWithDetails>>
{
    private readonly ISensorRepository _sensorRepository;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IHostRepository _hostRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServiceProvider _serviceProvider;

    public CreateSensorCommandHandler(
        ISensorRepository sensorRepository,
        IUnitOfWork unitOfWork,
        IDeviceRepository deviceRepository,
        IHostRepository hostRepository,
        IServiceProvider serviceProvider
    )
    {
        _sensorRepository = sensorRepository;
        _unitOfWork = unitOfWork;
        _deviceRepository = deviceRepository;
        _hostRepository = hostRepository;
        _serviceProvider = serviceProvider;
    }

    private async Task<ErrorOr<(ISensorDetail, SensorDetail)>> CreateDockerSensorAsync(
        Host host,
        CreateDockerSensorDetailCommand command,
        CancellationToken cancellationToken = default
    )
    {
        if (host.Type != HostType.DOCKER)
        {
            return Errors.Host.HostAndSensorTypeNotMatch;
        }

        var createPortainerServiceResult = _serviceProvider.CreatePortainerService(host);
        if (createPortainerServiceResult.IsError)
        {
            return createPortainerServiceResult.Errors;
        }

        var containerStatus = await createPortainerServiceResult.Value.GetContainerInformationAsync(
            command.PortainerId,
            command.ContainerId,
            cancellationToken
        );
        if (containerStatus.IsError)
        {
            return containerStatus.Errors;
        }

        var scanPeriodAsTimeSpan = TimeSpan.FromMinutes(command.ScanPeriodInMinutes);
        var dockerSensor = DockerSensor.Create(
            command.PortainerId,
            command.ContainerId,
            scanPeriodAsTimeSpan
        );

        await _sensorRepository.AddDockerSensorAsync(dockerSensor, cancellationToken);

        return (dockerSensor, SensorDetail.CreateDockerSensorDetail(dockerSensor.Id));
    }

    private async Task<ErrorOr<(ISensorDetail, SensorDetail)>> CreateKafkaSensorAsync(
        Host host,
        CreateKafkaSensorDetailCommand command,
        CancellationToken cancellationToken = default
    )
    {
        if (host.Type != HostType.KAFKA)
        {
            return Errors.Host.HostAndSensorTypeNotMatch;
        }

        var createKafkaServiceResult = _serviceProvider.CreateKafkaService(host);
        if (createKafkaServiceResult.IsError)
        {
            return createKafkaServiceResult.Errors;
        }

        var topicsResult = createKafkaServiceResult.Value.ListTopics();
        if (topicsResult.IsError)
        {
            return topicsResult.Errors;
        }

        if (!topicsResult.Value.Contains(command.TopicName))
        {
            return Error.Validation(
                "Kafka.Topic.NotFound",
                $"Kafka topic '{command.TopicName}' does not exists."
            );
        }

        var createKafkaTopicNameResult = TopicName.Create(command.TopicName);
        if (createKafkaTopicNameResult.IsError)
        {
            return createKafkaTopicNameResult.Errors;
        }

        var timeToLiveInSecondsAsTimeSpan = TimeSpan.FromSeconds(command.TimeToLiveInSeconds);
        var kafkaSensor = KafkaSensor.Create(
            createKafkaTopicNameResult.Value,
            timeToLiveInSecondsAsTimeSpan,
            command.IdentifierKey,
            command.IdentifierValue
        );

        await _sensorRepository.AddKafkaSensorAsync(kafkaSensor, cancellationToken);

        return (kafkaSensor, SensorDetail.CreateKafkaSensorDetail(kafkaSensor.Id));
    }

    private async Task<ErrorOr<(ISensorDetail, SensorDetail)>> CreateMqttSensorAsync(
        Host host,
        CreateMqttSensorDetailCommand command,
        CancellationToken cancellationToken = default
    )
    {
        if (host.Type != HostType.MQTT)
        {
            return Errors.Host.HostAndSensorTypeNotMatch;
        }

        var createTopicNameResult = TopicName.Create(command.TopicName);
        if (createTopicNameResult.IsError)
        {
            return createTopicNameResult.Errors;
        }

        var timeToLiveInSecondsAsTimeSpan = TimeSpan.FromSeconds(command.TimeToLiveInSeconds);
        var mqttSensor = MqttSensor.Create(
            createTopicNameResult.Value,
            timeToLiveInSecondsAsTimeSpan
        );

        await _sensorRepository.AddMqttSensorAsync(mqttSensor, cancellationToken);

        return (mqttSensor, SensorDetail.CreateMqttSensorDetail(mqttSensor.Id));
    }

    private async Task<ErrorOr<(ISensorDetail, SensorDetail)>> CreateNodeRedSensorAsync(
        Host host,
        CreateNodeRedSensorDetailCommand command,
        CancellationToken cancellationToken = default
    )
    {
        if (host.Type != HostType.NODE_RED)
        {
            return Errors.Host.HostAndSensorTypeNotMatch;
        }

        // TODO: validate node-red flow exists

        var scanPeriodInMinutesAsTimeSpan = TimeSpan.FromMinutes(command.ScanPeriodInMinutes);
        var nodeRedSensor = NodeRedSensor.Create(command.FlowId, scanPeriodInMinutesAsTimeSpan);

        await _sensorRepository.AddNodeRedSensorAsync(nodeRedSensor, cancellationToken);

        return (nodeRedSensor, SensorDetail.CreateNodeRedSensorDetail(nodeRedSensor.Id));
    }

    private async Task<ErrorOr<(ISensorDetail, SensorDetail)>> CreatePingSensorAsync(
        Host host,
        CreatePingSensorDetailCommand command,
        CancellationToken cancellationToken = default
    )
    {
        if (host.Type != HostType.PING)
        {
            return Errors.Host.HostAndSensorTypeNotMatch;
        }

        var scanPeriodInMinutesAsTimeSpan = TimeSpan.FromMinutes(command.ScanPeriodInMinutes);
        var pingSensor = PingSensor.Create(scanPeriodInMinutesAsTimeSpan);

        await _sensorRepository.AddPingSensorAsync(pingSensor, cancellationToken);

        return (pingSensor, SensorDetail.CreatePingSensorDetail(pingSensor.Id));
    }

    public async Task<ErrorOr<SensorWithDetails>> Handle(
        CreateSensorCommand request,
        CancellationToken cancellationToken
    )
    {
        if (!Enum.IsDefined(typeof(SensorType), request.SensorType))
        {
            return Errors.Sensor.Invalid;
        }
        var sensorType = (SensorType)request.SensorType;
        var deviceComponentId = DeviceComponentId.Create(request.DeviceComponentId);
        var hostId = HostId.Create(request.HostId);
        var deviceId = DeviceId.Create(request.DeviceId);
        if (await _deviceRepository.FindByIdAsync(deviceId, cancellationToken) is not Device device)
        {
            return Errors.Device.NotFound;
        }

        if (await _hostRepository.FindByIdAsync(hostId, cancellationToken) is not Host host)
        {
            return Errors.Host.NotFoundWithHostId(hostId);
        }

        var deviceComponent = device.FindDeviceComponentById(deviceComponentId);
        if (deviceComponent.IsError)
        {
            return deviceComponent.Errors;
        }

        ErrorOr<(ISensorDetail, SensorDetail)> createSensorDetailResult =
            request.SensorDetail switch
            {
                CreateDockerSensorDetailCommand createDockerSensorDetail
                    => await CreateDockerSensorAsync(
                        host,
                        createDockerSensorDetail,
                        cancellationToken
                    ),
                CreateKafkaSensorDetailCommand createKafkaSensorDetail
                    => await CreateKafkaSensorAsync(
                        host,
                        createKafkaSensorDetail,
                        cancellationToken
                    ),
                CreateNodeRedSensorDetailCommand createNodeRedSensorDetail
                    => await CreateNodeRedSensorAsync(
                        host,
                        createNodeRedSensorDetail,
                        cancellationToken
                    ),
                CreateMqttSensorDetailCommand createMqttSensorDetail
                    => await CreateMqttSensorAsync(host, createMqttSensorDetail, cancellationToken),
                CreatePingSensorDetailCommand createPingSensorDetailCommand
                    => await CreatePingSensorAsync(
                        host,
                        createPingSensorDetailCommand,
                        cancellationToken
                    ),
                _ => Errors.Sensor.Invalid
            };
        if (createSensorDetailResult.IsError)
        {
            return createSensorDetailResult.Errors;
        }

        (ISensorDetail sensorInterface, SensorDetail sensorDetail) = createSensorDetailResult.Value;
        var description = request.Description;

        var sensor = Sensor.Create(deviceComponentId, hostId, description, sensorDetail);
        await _sensorRepository.AddAsync(sensor, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new SensorWithDetails(
            sensor.Id,
            sensor.Description,
            sensor.Status,
            sensor.StatusText,
            (int)sensor.SensorDetail.Type,
            sensor.IsActive,
            sensor.SensorDetail.DetailId,
            sensor.LastCheckOnUtc,
            sensor.CreatedOnUtc,
            sensor.ModifiedOnUtc,
            sensorInterface,
            device,
            deviceComponent.Value,
            host
        );
    }
}
