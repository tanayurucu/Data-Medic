using DataMedic.Domain.Sensors;
using DataMedic.Domain.Sensors.Entities;
using DataMedic.Worker.Jobs.KafkaScanner.Models;

namespace DataMedic.Worker.Jobs.KafkaScanner;

public interface IKafkaScanService
{
    Task<List<KafkaSensor>> GetKafkaSensorsByHost(CancellationToken cancellationToken = default);
    Task<List<Sensor>> GetTypeKafkaSensors(CancellationToken cancellationToken = default);
    public Task<List<SensorWithKafkaDetails>> GetJoinedKafkaSensorDetailsAsync();
    Task UpdateSensorStatusByDetailIdAsync(Guid idValue, bool status);
}