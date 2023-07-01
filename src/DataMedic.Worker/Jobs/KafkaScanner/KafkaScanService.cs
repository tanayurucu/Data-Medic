using System.Diagnostics.CodeAnalysis;

using DataMedic.Domain.Sensors;
using DataMedic.Domain.Sensors.Entities;
using DataMedic.Domain.Sensors.ValueObjects;
using DataMedic.Persistence;
using DataMedic.Worker.Jobs.KafkaScanner.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using StackExchange.Redis;

namespace DataMedic.Worker.Jobs.KafkaScanner;

public class KafkaScanService : IKafkaScanService
{
    private readonly DataMedicDbContext _dbContext;
    private readonly ILogger<KafkaScanService> _logger;
    
    public KafkaScanService(DataMedicDbContext dbContext, ILogger<KafkaScanService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<List<KafkaSensor>> GetKafkaSensorsByHost(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<KafkaSensor>().ToListAsync(cancellationToken);
    }

    public async Task<List<Sensor>> GetTypeKafkaSensors(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Sensor>().Where(a => a.SensorDetail.Type == SensorType.KAFKA).ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<SensorWithKafkaDetails>> GetJoinedKafkaSensorDetailsAsync()
    {
        return await (from sensor in _dbContext.Set<Sensor>()
            where sensor.SensorDetail.Type == SensorType.KAFKA
            join kafkaSensor in _dbContext.Set<KafkaSensor>() on sensor.SensorDetail.DetailId equals (Guid)kafkaSensor.Id
            select new SensorWithKafkaDetails
            {
                SensorId = sensor.Id,
                Status = sensor.Status,
                StatusText = sensor.StatusText,
                SensorDetail = kafkaSensor
            }).ToListAsync();
    }

    public async Task UpdateSensorStatusByDetailIdAsync(Guid idValue, bool status)
    {
        var sensor = _dbContext.Set<Sensor>().FirstOrDefault(a => a.SensorDetail.DetailId == idValue);
        sensor?.SetStatus(status);
        await _dbContext.SaveChangesAsync();
    }
}