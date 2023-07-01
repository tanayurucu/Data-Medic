using DataMedic.Domain.Sensors;
using DataMedic.Domain.Sensors.Entities;
using DataMedic.Domain.Sensors.ValueObjects;
using DataMedic.Persistence;
using DataMedic.Worker.Jobs.MqttScanner.Models;

using Microsoft.EntityFrameworkCore;

namespace DataMedic.Worker.Jobs.MqttScanner;

public class MqttRepository : IMqttRepository
{
    private readonly DataMedicDbContext _dbContext;
    private readonly ILogger<MqttRepository> _logger;

    public MqttRepository(ILogger<MqttRepository> logger, DataMedicDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<List<SensorWithMqttDetails>> GetJoinedMqttSensorDetailsAsync()
    {
        return await (from sensor in _dbContext.Set<Sensor>()
            where sensor.SensorDetail.Type == SensorType.MQTT
            join mqttSensor in _dbContext.Set<MqttSensor>() on sensor.SensorDetail.DetailId equals (Guid)mqttSensor.Id
            select new SensorWithMqttDetails
            {
                SensorId = sensor.Id,
                Status = sensor.Status,
                StatusText = sensor.StatusText,
                SensorDetail = mqttSensor
            }).ToListAsync();
    }

    public async Task UpdateSensorStatusByDetailIdAsync(Guid idValue, bool status)
    {
        var sensor = _dbContext.Set<Sensor>().FirstOrDefault(a => a.SensorDetail.DetailId == idValue);
        sensor?.SetStatus(status);
        await _dbContext.SaveChangesAsync();
    }
}