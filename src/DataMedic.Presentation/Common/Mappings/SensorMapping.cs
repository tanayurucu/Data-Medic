using DataMedic.Application.Sensors.Commands.CreateSensor;
using DataMedic.Application.Sensors.Commands.DeleteSensor;
using DataMedic.Application.Sensors.Commands.UpdateSensor;
using DataMedic.Application.Sensors.Models;
using DataMedic.Application.Sensors.Queries.GetSensorById;
using DataMedic.Application.Sensors.Queries.GetSensorTree;
using DataMedic.Contracts.Sensors;
using DataMedic.Domain.Sensors;
using DataMedic.Domain.Sensors.Entities;

using Mapster;

namespace DataMedic.Presentation.Common.Mappings;

/// <summary>
/// Mappings for Sensor
/// </summary>
public sealed class SensorMapping : IRegister
{
    /// <inheritdoc />
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<ICreateSensorDetailRequest, ICreateSensorDetailCommand>()
            .ConstructUsing(src => ConvertCreateSensorRequestToCommand(src));

        config
            .NewConfig<IUpdateSensorDetailRequest, IUpdateSensorDetailCommand>()
            .ConstructUsing(src => ConvertUpdateSensorRequestToCommand(src));

        config
            .NewConfig<ISensorDetail, ISensorDetailResponse>()
            .ConstructUsing(src => ConvertSensorDetailToResponse(src));

        config
            .NewConfig<SensorWithDetails, SensorWithDetailResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Host, src => src.Host)
            .Map(dest => dest.DeviceComponent, src => src.DeviceComponent)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.StatusText, src => src.StatusText)
            .Map(dest => dest.LastCheckOnUtc, src => src.LastCheckOnUtc)
            .Map(dest => dest.SensorDetail, src => src.SensorDetail)
            .Map(dest => dest.Type, src => src.Type)
            .Map(dest => dest.DetailId, src => src.DetailId);

        config
            .NewConfig<SensorWithSensorDetail, SensorWithSensorDetailResponse>()
            .Map(dest => dest.Id, src => (Guid)src.Id.Value)
            .Map(dest => dest.Type, src => (int)src.Type)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.StatusText, src => src.StatusText)
            .Map(dest => dest.LastCheckOnUtc, src => src.LastCheckOnUtc)
            .Map(dest => dest.HostId, src => src.HostId.Value)
            .Map(dest => dest.SensorDetail, src => src.SensorDetail);

        config
            .NewConfig<CreateSensorRequest, CreateSensorCommand>()
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.DeviceComponentId, src => src.DeviceComponentId)
            .Map(dest => dest.DeviceId, src => src.DeviceId)
            .Map(dest => dest.HostId, src => src.HostId)
            .Map(dest => dest.SensorDetail, src => src.SensorDetail)
            .Map(dest => dest.SensorType, src => src.SensorDetail.Type);

        config.NewConfig<Guid, GetSensorByIdQuery>().Map(dest => dest.SensorId, src => src);

        config
            .NewConfig<(Guid sensorId, UpdateSensorRequest request), UpdateSensorCommand>()
            .Map(dest => dest.SensorId, src => src.sensorId)
            .Map(dest => dest.HostId, src => src.request.HostId)
            .Map(dest => dest.IsActive, src => src.request.IsActive)
            .Map(dest => dest.SensorDetail, src => src.request.SensorDetail)
            .Map(dest => dest.Description, src => src.request.Description);

        config.NewConfig<Guid, DeleteSensorCommand>().Map(dest => dest.SensorId, src => src);

        config.NewConfig<GetSensorTreeQueryParameters, GetSensorTreeQuery>();

        config.NewConfig<SensorTree, SensorTreeResponse>();

        config.NewConfig<SensorTreeStatistics, SensorTreeStatisticsResponse>();

        config
            .NewConfig<SensorTreeDevice, SensorTreeDeviceResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.InventoryNumber, src => src.InventoryNumber.Value);

        config
            .NewConfig<SensorTreeDeviceComponent, SensorTreeDeviceComponentResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.IpAddress, src => src.IpAddress.Value);

        config
            .NewConfig<SensorTreeSensor, SensorTreeSensorResponse>()
            .Map(dest => dest.Id, src => src.Id.Value);
    }

    private IUpdateSensorDetailCommand ConvertUpdateSensorRequestToCommand(
        IUpdateSensorDetailRequest src
    ) =>
        src switch
        {
            UpdateDockerSensorDetailRequest updateDockerSensorRequest
                => new UpdateDockerSensorDetailCommand(
                    updateDockerSensorRequest.PortainerId,
                    updateDockerSensorRequest.ContainerId,
                    updateDockerSensorRequest.ScanPeriodInMinutes
                ),
            UpdateKafkaSensorDetailRequest updateKafkaSensorRequest
                => new UpdateKafkaSensorDetailCommand(
                    updateKafkaSensorRequest.TopicName,
                    updateKafkaSensorRequest.TimeToLiveInSeconds,
                    updateKafkaSensorRequest.IdentifierKey,
                    updateKafkaSensorRequest.IdentifierValue
                ),
            UpdateNodeRedSensorDetailRequest updateNodeRedSensorRequest
                => new UpdateNodeRedSensorDetailCommand(
                    updateNodeRedSensorRequest.FlowId,
                    updateNodeRedSensorRequest.ScanPeriodInMinutes
                ),
            UpdateMqttSensorDetailRequest updateMqttSensorRequest
                => new UpdateMqttSensorDetailCommand(
                    updateMqttSensorRequest.TopicName,
                    updateMqttSensorRequest.TimeToLiveInSeconds
                ),
            UpdatePingSensorDetailRequest updatePingSensorRequest
                => new UpdatePingSensorDetailCommand(updatePingSensorRequest.ScanPeriodInMinutes),
            _
                => throw new ArgumentOutOfRangeException(
                    "Invalid Update sensor request",
                    nameof(IUpdateSensorDetailRequest)
                )
        };

    private static ISensorDetailResponse ConvertSensorDetailToResponse(ISensorDetail src) =>
        src switch
        {
            DockerSensor dockerSensor
                => new DockerSensorDetailResponse(
                    dockerSensor.Id.Value,
                    dockerSensor.PortainerId,
                    dockerSensor.ContainerId,
                    Convert.ToInt32(dockerSensor.ScanPeriod.TotalMinutes),
                    dockerSensor.LastLog
                ),
            KafkaSensor kafkaSensor
                => new KafkaSensorDetailResponse(
                    kafkaSensor.Id.Value,
                    kafkaSensor.TopicName.Value,
                    Convert.ToInt32(kafkaSensor.TimeToLiveInSeconds.TotalSeconds),
                    kafkaSensor.IdentifierKey,
                    kafkaSensor.IdentifierValue
                ),
            NodeRedSensor nodeRedSensor
                => new NodeRedSensorDetailResponse(
                    nodeRedSensor.FlowId,
                    nodeRedSensor.LastErrorLog,
                    nodeRedSensor.Id.Value,
                    Convert.ToInt32(nodeRedSensor.ScanPeriodInMinutes.TotalMinutes)
                ),
            MqttSensor mqttSensor
                => new MqttSensorDetailResponse(
                    mqttSensor.Id.Value,
                    mqttSensor.TopicName.Value,
                    Convert.ToInt32(mqttSensor.TimeToLiveInSeconds.TotalSeconds)
                ),
            PingSensor pingSensor
                => new PingSensorDetailResponse(
                    Convert.ToInt32(pingSensor.ScanPeriod.TotalMinutes),
                    pingSensor.Id.Value
                ),
            _
                => throw new ArgumentOutOfRangeException(
                    "Invalid sensor detail",
                    nameof(ISensorDetail)
                ),
        };

    private static ICreateSensorDetailCommand ConvertCreateSensorRequestToCommand(
        ICreateSensorDetailRequest src
    ) =>
        src switch
        {
            CreateDockerSensorDetailRequest createDockerSensorRequest
                => new CreateDockerSensorDetailCommand(
                    createDockerSensorRequest.PortainerId,
                    createDockerSensorRequest.ContainerId,
                    createDockerSensorRequest.ScanPeriodInMinutes
                ),
            CreateKafkaSensorDetailRequest createKafkaSensorRequest
                => new CreateKafkaSensorDetailCommand(
                    createKafkaSensorRequest.TopicName,
                    createKafkaSensorRequest.TimeToLiveInSeconds,
                    createKafkaSensorRequest.IdentifierKey,
                    createKafkaSensorRequest.IdentifierValue
                ),
            CreateNodeRedSensorDetailRequest createNodeRedSensorRequest
                => new CreateNodeRedSensorDetailCommand(
                    createNodeRedSensorRequest.FlowId,
                    createNodeRedSensorRequest.ScanPeriodInMinutes
                ),
            CreateMqttSensorDetailRequest createMqttSensorRequest
                => new CreateMqttSensorDetailCommand(
                    createMqttSensorRequest.TopicName,
                    createMqttSensorRequest.TimeToLiveInSeconds
                ),
            CreatePingSensorDetailRequest createPingSensorRequest
                => new CreatePingSensorDetailCommand(createPingSensorRequest.ScanPeriodInMinutes),
            _
                => throw new ArgumentOutOfRangeException(
                    "Invalid create sensor request",
                    nameof(ICreateSensorDetailRequest)
                )
        };
}
