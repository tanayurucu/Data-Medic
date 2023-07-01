using DataMedic.Application.Common.Interfaces.Infrastructure;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Worker.Jobs.MqttScanner.Models;
using Hangfire;
using Newtonsoft.Json;

using StackExchange.Redis;

namespace DataMedic.Worker.Jobs.MqttScanner;

public class CheckMqttStatus : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IDatabase _cache;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IEmailService _emailService;
    private readonly ILogger<CheckMqttStatus> _logger;

    public CheckMqttStatus(IServiceScopeFactory serviceScopeFactory, IDatabase cache, IDateTimeProvider dateTimeProvider, IEmailService emailService, ILogger<CheckMqttStatus> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _cache = cache;
        _dateTimeProvider = dateTimeProvider;
        _emailService = emailService;
        _logger = logger;
    }
    [Obsolete("Obsolete")]
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogWarning("Mqtt Scanner Started!");
        RecurringJob.AddOrUpdate(() => CheckTopicStatus(), "*/50 * * * * *");
        return Task.CompletedTask;
    }

    public async Task CheckTopicStatus()
    {
        var status = true;
        using var scope = _serviceScopeFactory.CreateScope();
        var mqttScanService = scope.ServiceProvider.GetRequiredService<IMqttRepository>();
        var mqttSensors = await mqttScanService.GetJoinedMqttSensorDetailsAsync();
        foreach (var sensor in mqttSensors)
        {
            var sensorTopicName = sensor.SensorDetail.TopicName.Value;
            var cacheKey = sensor.SensorDetail.TopicName.Value;
            var cacheValue = _cache.HashGet("mqtt",cacheKey);
            if (!cacheValue.HasValue)
            {
                await mqttScanService.UpdateSensorStatusByDetailIdAsync(sensor.SensorDetail.Id.Value, false);
                continue;
            }
            var deserializedObj = JsonConvert.DeserializeObject<MqttMessageDetail>(cacheValue!);
            if (deserializedObj != null && _dateTimeProvider.UtcNow - deserializedObj.Date > deserializedObj.TimeToLiveInSeconds && deserializedObj.Status != false)
            {
                status = false;
                await mqttScanService.UpdateSensorStatusByDetailIdAsync(sensor.SensorDetail.Id.Value, false);
                var emailRepository = scope.ServiceProvider.GetRequiredService<IEmailRepository>();
                var mailingList = await emailRepository.GetMailingListForSensorIdAsync(sensor.SensorId);
                await _emailService.SendEmailAsync(
                    mailingList.ConvertAll(email => email.Value),
                    "Data Medic: Mqtt Message Not receiving",
                    "topic: " + sensorTopicName + " device:" + cacheKey + " didn't receive any data for determined duration.",
                    CancellationToken.None);
            }else if (sensor.Status != true)
            {
                status = true;
                await mqttScanService.UpdateSensorStatusByDetailIdAsync(sensor.SensorDetail.Id.Value, true);
            }
            var jsonString = JsonConvert.SerializeObject(new
            {
                receiveTime = deserializedObj.Date,
                message = deserializedObj.Message,
                timeToLiveInSeconds = deserializedObj.TimeToLiveInSeconds,
                Topic = cacheKey,
                Status = status
            });
            _cache.HashSet("mqtt", cacheKey, jsonString);
        }
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        RecurringJob.RemoveIfExists(nameof(CheckMqttStatus.CheckTopicStatus));
        return Task.CompletedTask;
    }
}