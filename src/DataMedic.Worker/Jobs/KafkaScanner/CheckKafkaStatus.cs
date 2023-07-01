using DataMedic.Application.Common.Interfaces.Infrastructure;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Worker.Jobs.KafkaScanner.Models;

using StackExchange.Redis;

using Hangfire;
using Microsoft.IdentityModel.Tokens;

using Newtonsoft.Json;

namespace DataMedic.Worker.Jobs.KafkaScanner;

public class CheckKafkaStatus : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IDatabase _cache;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IEmailService _emailService;
    private readonly ILogger<CheckKafkaStatus> _logger;

    public CheckKafkaStatus(IServiceScopeFactory serviceScopeFactory, IDatabase cache, IDateTimeProvider dateTimeProvider, IEmailService emailService, ILogger<CheckKafkaStatus> logger)
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
        _logger.LogWarning("Kafka Scanner Started!");
        RecurringJob.AddOrUpdate(() => CheckTopicStatus(), "*/50 * * * * *");
        return Task.CompletedTask;
    }

    public async Task CheckTopicStatus()
    {
        var status = true;
        using var scope = _serviceScopeFactory.CreateScope();
        var kafkaScanService = scope.ServiceProvider.GetRequiredService<IKafkaScanService>();
        var kafkaSensors = await kafkaScanService.GetJoinedKafkaSensorDetailsAsync();
        foreach (var sensor in kafkaSensors)
        {
            var sensorTopicName = sensor.SensorDetail.TopicName.Value;
            var cacheKey = sensor.SensorDetail.IdentifierValue;
            if (sensor.SensorDetail.IdentifierValue.IsNullOrEmpty())
            {
                cacheKey = sensorTopicName;
            }
            var cacheValue = _cache.HashGet("kafka",cacheKey);
            if (!cacheValue.HasValue)
            {
                await kafkaScanService.UpdateSensorStatusByDetailIdAsync(sensor.SensorDetail.Id.Value, false);
                continue;
            }
            var deserializedObj = JsonConvert.DeserializeObject<KafkaMessageDetail>(cacheValue!);
            if (deserializedObj != null && _dateTimeProvider.UtcNow - deserializedObj.RecieveTime > deserializedObj.TimeToLiveInSeconds && deserializedObj.Status != false)
            {
                status = false;
                await kafkaScanService.UpdateSensorStatusByDetailIdAsync(sensor.SensorDetail.Id.Value, false);
                var emailRepository = scope.ServiceProvider.GetRequiredService<IEmailRepository>();
                var mailingList = await emailRepository.GetMailingListForSensorIdAsync(sensor.SensorId);
                await _emailService.SendEmailAsync(
                    mailingList.ConvertAll(email => email.Value),
                    "Data Medic: Kafka Message Not receiving",
                    "topic: " + sensorTopicName + " device:" + cacheKey + " didn't receive any data for determined duration.",
                    CancellationToken.None);
            }else if (sensor.Status != true)
            {
                status = true;
                await kafkaScanService.UpdateSensorStatusByDetailIdAsync(sensor.SensorDetail.Id.Value, true);
            }
            var jsonString = JsonConvert.SerializeObject(new
            {
                offset = deserializedObj.Offset,
                receiveTime = deserializedObj.RecieveTime,
                lastMessageRecieved = deserializedObj.LastMessageRecieved,
                timeToLiveInSeconds = deserializedObj.TimeToLiveInSeconds,
                deviceName = cacheKey,
                status = status
            });
            _cache.HashSet("kafka", cacheKey, jsonString);
        }
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        RecurringJob.RemoveIfExists(nameof(CheckKafkaStatus.CheckTopicStatus));
        return Task.CompletedTask;
    }
}