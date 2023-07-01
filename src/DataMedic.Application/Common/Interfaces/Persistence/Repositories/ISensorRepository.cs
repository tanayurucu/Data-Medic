using DataMedic.Application.Sensors.Models;
using DataMedic.Domain.ControlSystems.ValueObjects;
using DataMedic.Domain.Departments.ValueObjects;
using DataMedic.Domain.DeviceGroups.ValueObjects;
using DataMedic.Domain.Devices.ValueObjects;
using DataMedic.Domain.OperatingSystems.ValueObjects;
using DataMedic.Domain.Sensors;
using DataMedic.Domain.Sensors.Entities;
using DataMedic.Domain.Sensors.ValueObjects;
using ErrorOr;

namespace DataMedic.Application.Common.Interfaces.Persistence.Repositories;

public interface ISensorRepository : IAsyncRepository<Sensor, SensorId>
{
    Task AddDockerSensorAsync(
        DockerSensor dockerSensor,
        CancellationToken cancellationToken = default
    );
    Task AddKafkaSensorAsync(
        KafkaSensor kafkaSensor,
        CancellationToken cancellationToken = default
    );
    Task AddMqttSensorAsync(MqttSensor mqttSensor, CancellationToken cancellationToken = default);
    Task<Dictionary<string,int>> FindKafkaTopicNamesByHostDistinct(CancellationToken cancellationToken = default);
    Task<List<KafkaSensor>> FindKafkaSensorsByHostId(CancellationToken cancellationToken = default);
    Task<List<SensorWithKafkaDetails>> GetJoinedKafkaSensorDetails(CancellationToken cancellationToken = default);
    Task UpdatePingStatus(Guid sensorId, bool status, CancellationToken cancellationToken = default);
    Task UpdateDockerStatus(Guid sensorId, bool status, string containerDetailLogs,CancellationToken cancellationToken = default);
    Task UpdateNoderedStatus(Guid sensorId, string? lastErrorLog, bool status, CancellationToken cancellationToken = default);
    Task UpdateKafkaSensor(Guid sensorId, bool status, CancellationToken cancellationToken);

    public Task UpdateMqttStatus(Guid sensorId, bool status,
        CancellationToken cancellationToken = default);
    
    Task AddNodeRedSensorAsync(
        NodeRedSensor nodeRedSensor,
        CancellationToken cancellationToken = default
    );
    Task AddPingSensorAsync(PingSensor pingSensor, CancellationToken cancellationToken = default);
    Task<List<SensorWithSensorDetail>> FindAllSensorsByDeviceComponentIdAsync(
        DeviceComponentId deviceComponentId,
        CancellationToken cancellationToken = default
    );
    Task<ISensorDetail?> FindSensorDetailByIdAsync(
        SensorDetail sensorDetail,
        CancellationToken cancellationToken = default
    );

    Task<DockerSensor?> FindDockerSensorDetailByIdAsync(
        DockerSensorId dockerSensorId,
        CancellationToken cancellationToken = default
    );

    Task<KafkaSensor?> FindKafkaSensorDetailByIdAsync(
        KafkaSensorId kafkaSensorId,
        CancellationToken cancellationToken = default
    );

    Task<MqttSensor?> FindMqttSensorDetailByIdAsync(
        MqttSensorId mqttSensorId,
        CancellationToken cancellationToken = default
    );

    Task<NodeRedSensor?> FindNodeRedSensorDetailByIdAsync(
        NodeRedSensorId nodeRedSensorId,
        CancellationToken cancellationToken = default
    );

    Task<PingSensor?> FindPingSensorDetailByIdAsync(
        PingSensorId pingSensorId,
        CancellationToken cancellationToken = default
    );
    void RemoveSensorDetail(ISensorDetail sensorDetail);

    Task<SensorTree> GetSensorTreeAsync(
        bool showOnlyUpSensors,
        bool showOnlyDownSensors,
        bool showOnlyInactiveSensors,
        string searchString,
        ControlSystemId controlSystemId,
        OperatingSystemId operatingSystemId,
        DeviceGroupId deviceGroupId,
        DepartmentId departmentId,
        CancellationToken cancellationToken = default
    );
}
