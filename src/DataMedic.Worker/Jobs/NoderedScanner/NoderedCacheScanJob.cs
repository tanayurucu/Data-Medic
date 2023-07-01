
using DataMedic.Domain.Sensors.ValueObjects;
using DataMedic.Worker.Models;

using Newtonsoft.Json;
using Hangfire;
using StackExchange.Redis;

namespace DataMedic.Worker.Jobs.NoderedScanner;

public class NoderedCacheScanJob : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IDatabase _cache;

    public NoderedCacheScanJob(IServiceScopeFactory serviceScopeFactory, IDatabase cache)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _cache = cache;
    }
    static IDatabase CreateRedisClient()
    {
        return ConnectionMultiplexer.Connect("10.50.197.32").GetDatabase();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        RecurringJob.AddOrUpdate(() => ScanCache(), "*/5 * * * * *");
        return Task.CompletedTask;
    }

    public async Task ScanCache()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var noderedScanService = scope.ServiceProvider.GetRequiredService<INoderedScanService>();
        var noderedSensors = noderedScanService.GetNoderedSensors();
        var hashKeys = _cache.HashKeys("nodered");
        foreach (var key in hashKeys)
        {
            var keyGuid = new Guid(key.ToString());
            var sensorId = SensorId.Create(keyGuid);
            var value = _cache.HashGet("nodered", key);
            var responseObj = JsonConvert.DeserializeObject<NoderedErrorResponseModel>(value);
            var sensor = noderedSensors.FirstOrDefault(a => a.Id == sensorId);
            if (sensor != null && sensor.Status && responseObj.ErrorList.Any())
            {
                noderedScanService.UpdateSensorStatus(sensorId, false);
            }
            else if (sensor != null && sensor.Status == false && !responseObj.ErrorList.Any())
            {
                noderedScanService.UpdateSensorStatus(sensorId, true);
            }
        }
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        RecurringJob.RemoveIfExists(nameof(NoderedCacheScanJob.ScanCache));
        return Task.CompletedTask;
    }
}