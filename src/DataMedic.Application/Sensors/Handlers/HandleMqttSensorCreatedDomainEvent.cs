using System.Diagnostics;
using System.Text;

using DataMedic.Application.Common.Interfaces.Infrastructure;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Domain.Sensors.ValueObjects;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using StackExchange.Redis;

using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

using Newtonsoft.Json;

using IServiceProvider = DataMedic.Application.Common.Interfaces.Infrastructure.IServiceProvider;

namespace DataMedic.Application.Sensors.Handlers;

public class HandleMqttSensorCreatedDomainEvent : IHandleMqttSensorCreatedDomainEvent
{
    private readonly IDatabase _cache;
    private readonly ILogger<HandleMqttSensorCreatedDomainEvent> _logger;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IManagedMqttClient mqttClient;
    private MqttMessageInfo _latestMessageInfo;
    private bool _latestStatus;
    private readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(10);
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IServiceProvider _serviceProvider;
    
    public HandleMqttSensorCreatedDomainEvent(IDatabase cache, ILogger<HandleMqttSensorCreatedDomainEvent> logger, IDateTimeProvider dateTimeProvider, IServiceScopeFactory serviceScopeFactory, IServiceProvider serviceProvider)
    {
        _cache = cache;
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
        _serviceScopeFactory = serviceScopeFactory;
        _serviceProvider = serviceProvider;
        var factory = new MqttFactory();
        mqttClient = factory.CreateManagedMqttClient();
    }

    [Obsolete("Obsolete")]
    public async Task Handle(Guid sensorId, string topicName, Guid hostId, TimeSpan timeToLiveInSeconds,CancellationToken cancellationToken)
    {
        var options = new ManagedMqttClientOptionsBuilder()
            .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
            .WithClientOptions(new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .WithCleanSession()
                .Build())
            .Build();
        mqttClient.ApplicationMessageReceivedAsync += async e =>
        {
            try
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                await HandleRecievedMessage(
                    e.ApplicationMessage.Topic,
                    Encoding.UTF8.GetString(e.ApplicationMessage.Payload),timeToLiveInSeconds, cancellationToken);
                stopWatch.Stop();
                _logger.LogInformation(
                    "MQTT message handled in {@ElapsedMilliseconds} ms.",
                    stopWatch.ElapsedMilliseconds
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Exception occurred while handling MQTT message. {@Exception}",
                    ex.ToString()
                );
            }
        };
        _logger.LogWarning("Mqtt client started for topic:" + topicName);
        await mqttClient.StartAsync(options);
        await mqttClient.SubscribeAsync(topicName);
        await CheckTimeToLiveExpiration(sensorId, timeToLiveInSeconds, cancellationToken);
    }

    private Task HandleRecievedMessage(string topic, string message, TimeSpan timeToLiveInSeconds, CancellationToken cancellationToken = default)
    {
        var date = _dateTimeProvider.UtcNow;
        var messageInfo = new MqttMessageInfo
        {
            Date = date,
            Message = message,
            Topic = topic
        };
        _latestMessageInfo = messageInfo;
        return Task.CompletedTask;
    }
    private async Task CheckTimeToLiveExpiration(Guid sensorId, TimeSpan timeToLiveInSeconds, CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(_checkInterval, cancellationToken);

            if (_latestMessageInfo != null)
            {
                var timeSinceLastMessage = _dateTimeProvider.UtcNow - _latestMessageInfo.Date;

                if (timeSinceLastMessage >= timeToLiveInSeconds)
                {
                    if (_latestStatus)
                    {
                        using var scope = _serviceScopeFactory.CreateScope();
                        var sensorRepository = scope.ServiceProvider.GetRequiredService<ISensorRepository>();
                        _latestStatus = false;
                        await sensorRepository.UpdateMqttStatus(sensorId, false, cancellationToken);
                        var sensorIdObj = SensorId.Create(sensorId);
                        var emailRepository = scope.ServiceProvider.GetRequiredService<IEmailRepository>();
                        var mailingList = await emailRepository.GetMailingListForSensorIdAsync(sensorIdObj, cancellationToken);
                        var emailService = _serviceProvider.CreateEmailService().Value;
                        await emailService.SendEmailAsync(
                            mailingList.ConvertAll(email => email.Value),
                            "Data Medic: Mqtt sensor detect an error",
                            "Mqtt Topic: " + _latestMessageInfo.Topic + " don't receive message",
                            CancellationToken.None);
                    }
                }
                else
                {
                    if (_latestStatus == false)
                    {
                        using var scope = _serviceScopeFactory.CreateScope();
                        var sensorRepository = scope.ServiceProvider.GetRequiredService<ISensorRepository>();
                        await sensorRepository.UpdateMqttStatus(sensorId,true, cancellationToken);
                    }
                    _latestStatus = true;
                }
            }
        }
    }
}

public interface IHandleMqttSensorCreatedDomainEvent
{
    public Task Handle(Guid sensorId, string topicName, Guid hostId, TimeSpan timeToLiveInSeconds,CancellationToken cancellationToken = default);
}
public class MqttMessageInfo
{
    public DateTime Date { get; set; }
    public string Message { get; set; }
    public string Topic { get; set; }
}