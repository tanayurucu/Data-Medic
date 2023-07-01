using System.Text;

using Confluent.Kafka;

using DataMedic.Application.Common.Interfaces.Infrastructure;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Sensors.Models;
using DataMedic.Domain.Hosts.ValueObjects;
using DataMedic.Domain.Sensors.Events;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using StackExchange.Redis;

using IServiceProvider = DataMedic.Application.Common.Interfaces.Infrastructure.IServiceProvider;

namespace DataMedic.Application.Sensors.Handlers;

public class HandleKafkaSensorCreatedDomainEvent : IHandleKafkaSensorCreatedDomainEvent
{
    private readonly IDatabase _cache;
    private readonly ILogger<HandleKafkaSensorCreatedDomainEvent> _logger;
    private readonly IHostRepository _hostRepository;
    private readonly IServiceProvider _serviceProvider;
    private readonly IDateTimeProvider _dateTimeProvider;
    private KafkaMessageInfo _latestMessageInfo;
    private bool _latestStatus;
    private readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(10);
    private readonly ISensorRepository _sensorRepository;
    private List<KafkaMessageInfo?> _messagesAtTopic;

    public HandleKafkaSensorCreatedDomainEvent(IDatabase cache,
        ILogger<HandleKafkaSensorCreatedDomainEvent> logger, IHostRepository hostRepository, IServiceProvider serviceProvider, IDateTimeProvider dateTimeProvider, ISensorRepository sensorRepository)
    {
        _cache = cache;
        _logger = logger;
        _hostRepository = hostRepository;
        _serviceProvider = serviceProvider;
        _dateTimeProvider = dateTimeProvider;
        _sensorRepository = sensorRepository;
        _messagesAtTopic = new List<KafkaMessageInfo?>();
    }

    public async Task Handle(SensorCreatedDomainEvent notification, string topicName, string? identifierKey,
        string? identifierValue, TimeSpan timeToLiveInSeconds,Guid hostIdGuid,
        CancellationToken cancellationToken)
    {
        var hostId = HostId.Create(hostIdGuid);
        var host = await _hostRepository.FindByIdAsync(hostId, cancellationToken);
        var kafkaService = _serviceProvider.CreateKafkaService(host);
        if (kafkaService.IsError)
        {
            _logger.LogError(kafkaService.Errors.ToString());
            return;
        }
        var kafkaConfig = kafkaService.Value.GetConfig();
        var config = new ConsumerConfig
        {
            BootstrapServers = kafkaConfig.BootstrapServers,
            GroupId = "foo",
            AutoOffsetReset = AutoOffsetReset.Latest,
            SslCaPem = kafkaConfig.SslCaPem,
            SslCertificatePem = kafkaConfig.SslCertificatePem,
            SslKeyPem = kafkaConfig.SslKeyPem,
            SecurityProtocol = kafkaConfig.SecurityProtocol,
            SslKeyPassword = kafkaConfig.SslKeyPassword
        };
        //await CheckTimeToLiveExpiration(notification.SenosorId, topicName, identifierValue, timeToLiveInSeconds, cancellationToken);
        await Subscribe(topicName, identifierKey, timeToLiveInSeconds,config, CancellationToken.None);
    }

    private async Task Subscribe(string topicName, string? identifierKey, TimeSpan timeToLiveInSeconds,
        ConsumerConfig consumerConfig,
        CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig)
                    .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                    .Build();
                consumer.Subscribe(topicName);
                while (!cancellationToken.IsCancellationRequested)
                {
                    var result = consumer.Consume(cancellationToken);
                    var kafkaMessageDetail = new KafkaMessageDetail
                    {
                        Topic = result.Topic,
                        Offset = result.Offset.Value,
                        RecieveTime = _dateTimeProvider.UtcNow,
                        LastMessageRecieved = result.Message.Value,
                        TimeToLiveInSeconds = timeToLiveInSeconds,
                        Status = true
                    };
                    await HandleKafkaMessage(kafkaMessageDetail, identifierKey);
                }
            }
            catch (OperationCanceledException)
            {
                // Ignore the exception and exit gracefully.
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while subscribing to Kafka topic {topicName}. Retrying in 5 seconds... Error details: {ex.Message}");
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
        }
    }

    private Task HandleKafkaMessage(KafkaMessageDetail kafkaMessageDetail, string? identifierKey)
    {
        try
        {
            if (kafkaMessageDetail.LastMessageRecieved == null)
            {
                throw new InvalidOperationException();
            }

            var deviceName = kafkaMessageDetail.Topic;
            if (identifierKey != null)
            {
                var keyToRead = identifierKey;
                Dictionary<string, object> kafkaMessageDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, object>>(kafkaMessageDetail.LastMessageRecieved!)!;
                var deviceIdentifier = kafkaMessageDictionary.FirstOrDefault(a => a.Key == keyToRead);
                if (deviceIdentifier.Value != null)
                {
                    deviceName = (string)deviceIdentifier.Value;
                }
                else
                {
                    _logger.LogError($"Identifier key {keyToRead} not found for topic {kafkaMessageDetail.Topic}.");
                }
            }

            var jsonString = JsonConvert.SerializeObject(new
            {
                offset = kafkaMessageDetail.Offset,
                receiveTime = kafkaMessageDetail.RecieveTime,
                lastMessageRecieved = kafkaMessageDetail.LastMessageRecieved,
                timeToLiveInSeconds = kafkaMessageDetail.TimeToLiveInSeconds,
                deviceName,
                status = true
            });
            _cache.HashSet("kafka", deviceName, jsonString);
            // var messageInfo = new KafkaMessageInfo
            // {
            //     Date = kafkaMessageDetail.RecieveTime,
            //     Message = kafkaMessageDetail.LastMessageRecieved,
            //     Key = deviceName
            // };
            //_latestMessageInfo = messageInfo;
            //_messagesAtTopic.Add(messageInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
        }
        return Task.CompletedTask;
    }
    private async Task CheckTimeToLiveExpiration(Guid sensorId, string topic, string? identifierValue, TimeSpan timeToLiveInSeconds, CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(_checkInterval, cancellationToken);
            var device = topic;
            if (identifierValue != null)
            {
                device = identifierValue;
            }
            _latestMessageInfo = _messagesAtTopic.FirstOrDefault(a => a.Key == device);
            if (_latestMessageInfo != null)
            {
                var timeSinceLastMessage = _dateTimeProvider.UtcNow - _latestMessageInfo.Date;
                if (timeSinceLastMessage >= timeToLiveInSeconds)
                {
                    if (_latestStatus)
                    {
                        _latestStatus = false;
                        await _sensorRepository.UpdateKafkaSensor(sensorId,false, cancellationToken);
                    }
                }
                else
                {
                    if (_latestStatus == false)
                    { 
                        await _sensorRepository.UpdateKafkaSensor(sensorId,true, cancellationToken);
                    }
                    _latestStatus = true;
                }
            }
        }
    }
}

public interface IHandleKafkaSensorCreatedDomainEvent
{
    public Task Handle(SensorCreatedDomainEvent notification, string topicName, string? identifierKey,
        string? identifierValue, TimeSpan TimeToLiveInSeconds,
        Guid hostIdGuid,
        CancellationToken cancellationToken);
}
public class KafkaMessageInfo
{
    public DateTime Date { get; set; }
    public string Message { get; set; }
    public string Key { get; set; }
}