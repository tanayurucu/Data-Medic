using DataMedic.Worker.Jobs.MqttScanner.Models;

namespace DataMedic.Worker.Jobs.MqttScanner;

public interface IMqttRepository
{
    public Task<List<SensorWithMqttDetails>> GetJoinedMqttSensorDetailsAsync();
    Task UpdateSensorStatusByDetailIdAsync(Guid idValue, bool status);
}