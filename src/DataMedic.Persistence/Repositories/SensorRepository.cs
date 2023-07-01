using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Sensors.Models;
using DataMedic.Domain.Components;
using DataMedic.Domain.ControlSystems;
using DataMedic.Domain.ControlSystems.ValueObjects;
using DataMedic.Domain.Departments;
using DataMedic.Domain.Departments.ValueObjects;
using DataMedic.Domain.DeviceGroups;
using DataMedic.Domain.DeviceGroups.ValueObjects;
using DataMedic.Domain.Devices;
using DataMedic.Domain.Devices.ValueObjects;
using DataMedic.Domain.OperatingSystems.ValueObjects;
using DataMedic.Domain.Sensors;
using DataMedic.Domain.Sensors.Entities;
using DataMedic.Domain.Sensors.ValueObjects;
using DataMedic.Persistence.Common.Abstractions;

using Microsoft.EntityFrameworkCore;

using OperatingSystem = DataMedic.Domain.OperatingSystems.OperatingSystem;

namespace DataMedic.Persistence.Repositories;

internal sealed class SensorRepository : AsyncRepository<Sensor, SensorId>, ISensorRepository
{
    private ISensorRepository _sensorRepositoryImplementation;

    public SensorRepository(DataMedicDbContext dbContext)
        : base(dbContext) { }

    public async Task AddDockerSensorAsync(
        DockerSensor dockerSensor,
        CancellationToken cancellationToken = default
    ) => await _dbContext.Set<DockerSensor>().AddAsync(dockerSensor, cancellationToken);

    public async Task AddKafkaSensorAsync(
        KafkaSensor kafkaSensor,
        CancellationToken cancellationToken = default
    ) => await _dbContext.Set<KafkaSensor>().AddAsync(kafkaSensor, cancellationToken);

    public async Task AddMqttSensorAsync(
        MqttSensor mqttSensor,
        CancellationToken cancellationToken = default
    ) => await _dbContext.Set<MqttSensor>().AddAsync(mqttSensor, cancellationToken);

    public async Task AddNodeRedSensorAsync(
        NodeRedSensor nodeRedSensor,
        CancellationToken cancellationToken = default
    ) => await _dbContext.Set<NodeRedSensor>().AddAsync(nodeRedSensor, cancellationToken);

    public async Task AddPingSensorAsync(
        PingSensor pingSensor,
        CancellationToken cancellationToken = default
    ) => await _dbContext.Set<PingSensor>().AddAsync(pingSensor, cancellationToken);

    public Task<List<SensorWithSensorDetail>> FindAllSensorsByDeviceComponentIdAsync(
        DeviceComponentId deviceComponentId,
        CancellationToken cancellationToken
    )
    {
        var result = new List<SensorWithSensorDetail>();
        var sensors = _dbContext.Set<Sensor>().Where(a => a.DeviceComponentId == deviceComponentId).ToList();
        foreach (var s in sensors)
        {
            var sensorDetail = FindSensorDetail(s.SensorDetail);
            if (sensorDetail != null)
            {
                var detailView = new SensorWithSensorDetail(
                    s.Id,
                    s.SensorDetail.Type,
                    s.Description,
                    s.Status,
                    s.StatusText,
                    s.IsActive,
                    s.HostId,
                    s.LastCheckOnUtc,
                    s.CreatedOnUtc,
                    s.ModifiedOnUtc,
                    sensorDetail
                );
                result.Add(detailView);
            }
        }
        return Task.FromResult(result);
    }

    private ISensorDetail? FindSensorDetail(SensorDetail sensorDetail) =>
        sensorDetail switch
        {
            var dockerDetail when sensorDetail.Type == SensorType.DOCKER
                => _dbContext
                    .Set<DockerSensor>()
                    .FirstOrDefault(
                        dockerSensor =>
                            dockerSensor.Id == DockerSensorId.Create(dockerDetail.DetailId)
                    ),
            var kafkaDetail when sensorDetail.Type == SensorType.KAFKA
                => _dbContext
                    .Set<KafkaSensor>()
                    .FirstOrDefault(
                        kafkaSensor => kafkaSensor.Id == KafkaSensorId.Create(kafkaDetail.DetailId)
                    ),
            var mqttDetail when sensorDetail.Type == SensorType.MQTT
                => _dbContext
                    .Set<MqttSensor>()
                    .FirstOrDefault(
                        mqttSensor => mqttSensor.Id == MqttSensorId.Create(mqttDetail.DetailId)
                    ),
            var nodeRedDetail when sensorDetail.Type == SensorType.NODE_RED
                => _dbContext
                    .Set<NodeRedSensor>()
                    .FirstOrDefault(
                        nodeRedSensor =>
                            nodeRedSensor.Id == NodeRedSensorId.Create(nodeRedDetail.DetailId)
                    ),
            var pingDetail when sensorDetail.Type == SensorType.PING
                => _dbContext
                    .Set<PingSensor>()
                    .FirstOrDefault(
                        pingSensor => pingSensor.Id == PingSensorId.Create(pingDetail.DetailId)
                    ),
            _ => null
        };

    public async Task<ISensorDetail?> FindSensorDetailByIdAsync(
        SensorDetail sensorDetail,
        CancellationToken cancellationToken
    ) =>
        sensorDetail switch
        {
            var dockerDetail when sensorDetail.Type == SensorType.DOCKER
                => await _dbContext
                    .Set<DockerSensor>()
                    .FirstOrDefaultAsync(
                        dockerSensor =>
                            dockerSensor.Id == DockerSensorId.Create(dockerDetail.DetailId),
                        cancellationToken
                    ),
            var kafkaDetail when sensorDetail.Type == SensorType.KAFKA
                => await _dbContext
                    .Set<KafkaSensor>()
                    .FirstOrDefaultAsync(
                        kafkaSensor => kafkaSensor.Id == KafkaSensorId.Create(kafkaDetail.DetailId),
                        cancellationToken
                    ),
            var mqttDetail when sensorDetail.Type == SensorType.MQTT
                => await _dbContext
                    .Set<MqttSensor>()
                    .FirstOrDefaultAsync(
                        mqttSensor => mqttSensor.Id == MqttSensorId.Create(mqttDetail.DetailId),
                        cancellationToken
                    ),
            var nodeRedDetail when sensorDetail.Type == SensorType.NODE_RED
                => await _dbContext
                    .Set<NodeRedSensor>()
                    .FirstOrDefaultAsync(
                        noderedSensor =>
                            noderedSensor.Id == NodeRedSensorId.Create(nodeRedDetail.DetailId),
                        cancellationToken
                    ),
            var pingDetail when sensorDetail.Type == SensorType.PING
                => await _dbContext
                    .Set<PingSensor>()
                    .FirstOrDefaultAsync(
                        pingSensor => pingSensor.Id == PingSensorId.Create(pingDetail.DetailId),
                        cancellationToken
                    ),
            _ => null
        };

    public void RemoveSensorDetail(ISensorDetail sensorDetail)
    {
        object _ = sensorDetail switch
        {
            DockerSensor dockerSensor => _dbContext.Set<DockerSensor>().Remove(dockerSensor),
            KafkaSensor kafkaSensor => _dbContext.Set<KafkaSensor>().Remove(kafkaSensor),
            MqttSensor mqttSensor => _dbContext.Set<MqttSensor>().Remove(mqttSensor),
            NodeRedSensor noderedSensor => _dbContext.Set<NodeRedSensor>().Remove(noderedSensor),
            PingSensor pingSensor => _dbContext.Set<PingSensor>().Remove(pingSensor),
            _ => throw new ArgumentOutOfRangeException(nameof(sensorDetail))
        };
    }

    public Task<DockerSensor?> FindDockerSensorDetailByIdAsync(
        DockerSensorId dockerSensorId,
        CancellationToken cancellationToken = default
    ) =>
        _dbContext
            .Set<DockerSensor>()
            .FirstOrDefaultAsync(
                dockerSensor => dockerSensor.Id == dockerSensorId,
                cancellationToken
            );

    public Task<KafkaSensor?> FindKafkaSensorDetailByIdAsync(
        KafkaSensorId kafkaSensorId,
        CancellationToken cancellationToken = default
    ) =>
        _dbContext
            .Set<KafkaSensor>()
            .FirstOrDefaultAsync(kafkaSensor => kafkaSensor.Id == kafkaSensorId, cancellationToken);

    public Task<Dictionary<string, int>> FindKafkaTopicNamesByHostDistinct(CancellationToken cancellationToken = default)
    {
        var timeLimitByTopicDict = _dbContext.Set<KafkaSensor>().GroupBy(e => e.TopicName)
            .Select(g => new
            {
                g.Key,
                TimeToLiveInSeconds = g.Min(ks => ks.TimeToLiveInSeconds)
            })
            .ToDictionary(k => k.Key.Value, v => v.TimeToLiveInSeconds.Seconds);
        return Task.FromResult(timeLimitByTopicDict);
    }
    public Task<List<KafkaSensor>> FindKafkaSensorsByHostId(CancellationToken cancellationToken = default)
    {
        var kafkaSensors = _dbContext.Set<KafkaSensor>().ToList();
        return Task.FromResult(kafkaSensors);
    }

    public Task<List<SensorWithKafkaDetails>> GetJoinedKafkaSensorDetails(CancellationToken cancellationToken = default)
    {
        return Task.FromResult((from sensor in _dbContext.Set<Sensor>()
                                where sensor.SensorDetail.Type == SensorType.KAFKA
                                join kafkaSensor in _dbContext.Set<KafkaSensor>() on sensor.SensorDetail.DetailId equals (Guid)kafkaSensor.Id
                                select new SensorWithKafkaDetails
                                {
                                    Status = sensor.Status,
                                    StatusText = sensor.StatusText,
                                    SensorDetail = kafkaSensor
                                }).ToList());
    }

    public Task UpdatePingStatus(Guid sensorId, bool status, CancellationToken cancellationToken = default)
    {
        var id = SensorId.Create(sensorId);
        var entity = _dbContext.Set<Sensor>().FirstOrDefault(a => a.Id == id);
        // var detailId = PrtgSensorId.Create(entity!.SensorDetailId);
        // var pingSensor = _dbContext.Set<PrtgSensor>().FirstOrDefault(a => a.Id == detailId);
        entity?.SetStatus(status);
        _dbContext.SaveChangesAsync(cancellationToken);
        return Task.CompletedTask;
    }

    public Task UpdateDockerStatus(Guid sensorId, bool status, string containerDetailLogs,
        CancellationToken cancellationToken = default)
    {
        var id = SensorId.Create(sensorId);
        var entity = _dbContext.Set<Sensor>().FirstOrDefault(a => a.Id == id);
        var detailId = DockerSensorId.Create(entity!.SensorDetail.DetailId);
        var dockerSensor = _dbContext.Set<DockerSensor>().FirstOrDefault(a => a.Id == detailId);
        dockerSensor?.SetLastLog(containerDetailLogs);
        entity.SetStatus(status);
        entity.SetStatusText(status == false ? "ERROR" : "Running");
        _dbContext.SaveChangesAsync(cancellationToken);
        return Task.CompletedTask;
    }

    public Task UpdateNoderedStatus(Guid sensorId, string? lastErrorLog, bool status,
        CancellationToken cancellationToken = default)
    {
        var id = SensorId.Create(sensorId);
        var sensor = _dbContext.Set<Sensor>().FirstOrDefault(a => a.Id == id);
        if (sensor != null)
        {
            var detailId = NodeRedSensorId.Create(sensor.SensorDetail.DetailId);
            var noderedSensor = _dbContext.Set<NodeRedSensor>().FirstOrDefault(a => a.Id == detailId);
            if (lastErrorLog != null)
            {
                noderedSensor?.SetLastErrorLog(lastErrorLog);
            }
            sensor.SetStatus(status);
            _dbContext.SaveChangesAsync(cancellationToken);
        }
        return Task.CompletedTask;
    }

    public Task UpdateKafkaSensor(Guid sensorId, bool status, CancellationToken cancellationToken)
    {
        var id = SensorId.Create(sensorId);
        var sensor = _dbContext.Set<Sensor>().FirstOrDefault(a => a.Id == id);
        if (sensor != null)
        {
            sensor.SetStatus(status);
            _dbContext.SaveChangesAsync(cancellationToken);
        }
        return Task.CompletedTask;
    }

    public Task UpdateMqttStatus(Guid sensorId, bool status,
        CancellationToken cancellationToken = default)
    {
        var id = SensorId.Create(sensorId);
        var sensor = _dbContext.Set<Sensor>().FirstOrDefault(a => a.Id == id);
        if (sensor != null)
        {
            sensor.SetStatus(status);
            _dbContext.SaveChangesAsync(cancellationToken);
        }
        return Task.CompletedTask;
    }

    // Method to get the appropriate ISensorDetail object based on the sensor type
    private static ISensorDetail? GetSensorDetail(
        int type,
        DockerSensor? dockerSensor,
        KafkaSensor? kafkaSensor,
        MqttSensor? mqttSensor,
        NodeRedSensor? nodeRedSensor,
        PingSensor? prtgSensor
    )
    {
        switch (type)
        {
            case 0:
                return dockerSensor;
            case 1:
                return kafkaSensor;
            case 2:
                return mqttSensor;
            case 3:
                return nodeRedSensor;
            case 4:
                return prtgSensor;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), $"Not expected direction value: {type}");
        }
    }
    public Task<MqttSensor?> FindMqttSensorDetailByIdAsync(
        MqttSensorId mqttSensorId,
        CancellationToken cancellationToken = default
    ) =>
        _dbContext
            .Set<MqttSensor>()
            .FirstOrDefaultAsync(mqttSensor => mqttSensor.Id == mqttSensorId, cancellationToken);

    public Task<NodeRedSensor?> FindNodeRedSensorDetailByIdAsync(
        NodeRedSensorId nodeRedSensorId,
        CancellationToken cancellationToken = default
    ) =>
        _dbContext
            .Set<NodeRedSensor>()
            .FirstOrDefaultAsync(
                nodeRedSensor => nodeRedSensor.Id == nodeRedSensorId,
                cancellationToken
            );

    public Task<PingSensor?> FindPingSensorDetailByIdAsync(
        PingSensorId pingSensorId,
        CancellationToken cancellationToken = default
    ) =>
        _dbContext
            .Set<PingSensor>()
            .FirstOrDefaultAsync(
                pingSensor => pingSensor.Id == pingSensorId,
                cancellationToken: cancellationToken
            );

    public async Task<SensorTree> GetSensorTreeAsync(
        bool showOnlyUpSensors,
        bool showOnlyDownSensors,
        bool showOnlyInactiveSensors,
        string searchString,
        ControlSystemId controlSystemId,
        OperatingSystemId operatingSystemId,
        DeviceGroupId deviceGroupId,
        DepartmentId departmentId,
        CancellationToken cancellationToken = default
    )
    {
        var sensorTreeDevices = await (
            from device in _dbContext.Set<Device>()
            where
                string.IsNullOrWhiteSpace(searchString)
                || ((string)device.Name).Contains(searchString)
            where deviceGroupId.Value == Guid.Empty || device.DeviceGroupId == deviceGroupId
            where departmentId.Value == Guid.Empty || device.DepartmentId == departmentId
            join department in _dbContext.Set<Department>()
                on device.DepartmentId equals department.Id
            join deviceGroup in _dbContext.Set<DeviceGroup>()
                on device.DeviceGroupId equals deviceGroup.Id
            from deviceComponent in device.Components
            where
                operatingSystemId.Value == Guid.Empty
                || deviceComponent.OperatingSystemId == operatingSystemId
            where
                controlSystemId.Value == Guid.Empty
                || deviceComponent.ControlSystemId == controlSystemId
            join component in _dbContext.Set<Component>()
                on deviceComponent.ComponentId equals component.Id
            join operatingSystem in _dbContext.Set<OperatingSystem>()
                on deviceComponent.OperatingSystemId equals operatingSystem.Id
            join controlSystem in _dbContext.Set<ControlSystem>()
                on deviceComponent.ControlSystemId equals controlSystem.Id
            select new SensorTreeDevice(
                device.Id,
                device.Name,
                device.InventoryNumber,
                deviceGroup,
                department,
                new SensorTreeDeviceComponent(
                    deviceComponent.Id,
                    component,
                    deviceComponent.IpAddress,
                    operatingSystem,
                    controlSystem,
                    _dbContext
                        .Set<Sensor>()
                        .Where(sensor => sensor.DeviceComponentId == deviceComponent.Id)
                        .Where(sensor => !showOnlyUpSensors || (sensor.IsActive && sensor.Status))
                        .Where(
                            sensor => !showOnlyDownSensors || (sensor.IsActive && !sensor.Status)
                        )
                        .Where(sensor => !showOnlyInactiveSensors || (!sensor.IsActive))
                        .Select(
                            sensor =>
                                new SensorTreeSensor(
                                    sensor.Id,
                                    sensor.SensorDetail.Type,
                                    sensor.IsActive,
                                    sensor.Status
                                )
                        )
                        .ToList()
                )
            )
        ).ToListAsync(cancellationToken);

        return new SensorTree(
            new SensorTreeStatistics(
                sensorTreeDevices.Sum(
                    device =>
                        device.DeviceComponent.Sensors.Count(
                            sensor => sensor.IsActive && sensor.Status
                        )
                ),
                sensorTreeDevices.Sum(
                    device =>
                        device.DeviceComponent.Sensors.Count(
                            sensor => sensor.IsActive && !sensor.Status
                        )
                ),
                sensorTreeDevices.Sum(
                    device => device.DeviceComponent.Sensors.Count(sensor => !sensor.IsActive)
                )
            ),
            sensorTreeDevices
                .Where(
                    sensorTree =>
                        (!showOnlyUpSensors && !showOnlyDownSensors && !showOnlyInactiveSensors)
                        || sensorTree.DeviceComponent.Sensors.Count > 0
                )
                .ToList()
        );
    }
}
