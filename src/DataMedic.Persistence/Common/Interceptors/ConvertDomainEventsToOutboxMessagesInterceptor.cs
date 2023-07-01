using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

using DataMedic.Domain.Common.Abstractions;

using Hangfire;

using DataMedic.Application.Sensors.Handlers;
using DataMedic.Domain.Devices.Entities;
using DataMedic.Domain.Devices.ValueObjects;
using DataMedic.Domain.Hosts;
using DataMedic.Domain.Hosts.ValueObjects;
using DataMedic.Domain.Sensors.Entities;
using DataMedic.Domain.Sensors.Events;
using DataMedic.Domain.Sensors.ValueObjects;

using Hangfire.States;

using Microsoft.Extensions.DependencyInjection;

namespace DataMedic.Persistence.Common.Interceptors;

public sealed class ConvertDomainEventsToOutboxMessagesInterceptor : SaveChangesInterceptor
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IHandleMqttSensorCreatedDomainEvent _handleMqttSensorCreatedDomainEvent;

    public ConvertDomainEventsToOutboxMessagesInterceptor(IBackgroundJobClient backgroundJobClient,
        IServiceScopeFactory serviceScopeFactory,
        IHandleMqttSensorCreatedDomainEvent handleMqttSensorCreatedDomainEvent)
    {
        _backgroundJobClient = backgroundJobClient;
        _serviceScopeFactory = serviceScopeFactory;
        _handleMqttSensorCreatedDomainEvent = handleMqttSensorCreatedDomainEvent;
    }

    [Obsolete("Obsolete")]
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        DbContext? dbContext = eventData.Context;

        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var domainEvents = dbContext.ChangeTracker
            .Entries<IAggregateRoot>()
            .Select(x => x.Entity)
            .SelectMany(aggregateRoot =>
            {
                var domainEvents = aggregateRoot.DomainEvents.ToList();

                aggregateRoot.ClearDomainEvents();

                return domainEvents;
            }).ToList();
        foreach (var dEvent in domainEvents)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var handleKafkaSensorCreatedDomainEvent =
                scope.ServiceProvider.GetRequiredService<IHandleKafkaSensorCreatedDomainEvent>();
            var handlePingSensorCreatedDomainEvent =
                scope.ServiceProvider.GetRequiredService<IHandlePingSensorCreatedDomainEvent>();
            var handleDockerSensorCreatedDomainEvent =
                scope.ServiceProvider.GetRequiredService<IHandleDockerSensorCreatedDomainEvent>();
            var handleNoderedSensorCreatedDomainEvent =
                scope.ServiceProvider.GetRequiredService<IHandleNoderedSensorCreatedDomainEvent>();
            var dataMedicContext = scope.ServiceProvider.GetRequiredService<DataMedicDbContext>();
            switch (dEvent)
            {
                case SensorCreatedDomainEvent sensorCreatedDomainEvent:
                    if (sensorCreatedDomainEvent.Type == (int)SensorType.KAFKA)
                    {
                        var kafkaSensorId = KafkaSensorId.Create(sensorCreatedDomainEvent.DetailId);
                        var newKafkaSensor = dbContext.ChangeTracker.Entries<KafkaSensor>()
                            .FirstOrDefault(a => a.Entity.Id == kafkaSensorId)?.Entity;
                        var isSubscribedBefore = dataMedicContext.Set<KafkaSensor>()
                            .Any(a => a.TopicName == newKafkaSensor!.TopicName);
                        if (!isSubscribedBefore)
                        {
                            var topicName = newKafkaSensor!.TopicName.Value;
                            _backgroundJobClient.Enqueue(() =>
                                handleKafkaSensorCreatedDomainEvent.Handle(sensorCreatedDomainEvent,
                                    topicName, newKafkaSensor.IdentifierKey,
                                    newKafkaSensor.IdentifierValue, newKafkaSensor.TimeToLiveInSeconds,
                                    sensorCreatedDomainEvent.HostId,
                                    cancellationToken)
                            );
                        }
                    }

                    if (sensorCreatedDomainEvent.Type == (int)SensorType.PING)
                    {
                        var prtgSensorId = PingSensorId.Create(sensorCreatedDomainEvent.DetailId);
                        var deviceComponentId = DeviceComponentId.Create(sensorCreatedDomainEvent.DeviceComponentId);
                        var hostId = HostId.Create(sensorCreatedDomainEvent.HostId);
                        var newPingSensor = dbContext.ChangeTracker.Entries<PingSensor>()
                            .FirstOrDefault(a => a.Entity.Id == prtgSensorId)?.Entity;
                        var deviceComponentOfSensor = dbContext.ChangeTracker.Entries<DeviceComponent>()
                            .FirstOrDefault(a => a.Entity.Id == deviceComponentId)?.Entity;
                        var hostOfIp = dbContext.ChangeTracker.Entries<Host>()
                            .FirstOrDefault(a => a.Entity.Id == hostId)?.Entity;
                        var hostAdress = hostOfIp?.Uris[0].Value;
                        var ipAdress = deviceComponentOfSensor.IpAddress.Value;
                        var cronExpression = GetCronExpression(newPingSensor.ScanPeriod);
                        RecurringJob.AddOrUpdate(
                            sensorCreatedDomainEvent.SenosorId.ToString(),
                            () => handlePingSensorCreatedDomainEvent.Handle(sensorCreatedDomainEvent.SenosorId,
                                ipAdress, hostAdress, newPingSensor.ScanPeriod), cronExpression);
                    }

                    if (sensorCreatedDomainEvent.Type == (int)SensorType.DOCKER)
                    {
                        var dockerSensorId = DockerSensorId.Create(sensorCreatedDomainEvent.DetailId);
                        var newDockerSensor = dbContext.ChangeTracker.Entries<DockerSensor>()
                            .FirstOrDefault(a => a.Entity.Id == dockerSensorId)?.Entity;
                        var cronExpression = GetCronExpression(newDockerSensor.ScanPeriod);
                        RecurringJob.AddOrUpdate(
                            sensorCreatedDomainEvent.SenosorId.ToString(),
                            () => handleDockerSensorCreatedDomainEvent.Handle(sensorCreatedDomainEvent.SenosorId,
                                newDockerSensor!.PortainerId, newDockerSensor.ContainerId,
                                sensorCreatedDomainEvent.HostId)
                            , cronExpression
                        );
                    }

                    if (sensorCreatedDomainEvent.Type == (int)SensorType.NODE_RED)
                    {
                        var noderedSensorId = NodeRedSensorId.Create(sensorCreatedDomainEvent.DetailId);
                        var hostId = HostId.Create(sensorCreatedDomainEvent.HostId);
                        var newNoderedSensor = dbContext.ChangeTracker.Entries<NodeRedSensor>()
                            .FirstOrDefault(a => a.Entity.Id == noderedSensorId)?.Entity;
                        var hostOfNoderedSensor = dbContext.ChangeTracker.Entries<Host>()
                            .FirstOrDefault(a => a.Entity.Id == hostId)?.Entity;
                        var hostName = hostOfNoderedSensor?.Uris.FirstOrDefault()?.Value;
                        if (hostName != null)
                        {
                            var cronExpression = GetCronExpression(newNoderedSensor.ScanPeriodInMinutes);
                            RecurringJob.AddOrUpdate(
                                sensorCreatedDomainEvent.SenosorId.ToString(),
                                () => handleNoderedSensorCreatedDomainEvent.Handle(sensorCreatedDomainEvent.SenosorId
                                    , newNoderedSensor!.FlowId
                                    , hostName)
                                , cronExpression
                            );
                        }
                    }

                    if (sensorCreatedDomainEvent.Type == (int)SensorType.MQTT)
                    {
                        var mqttSensorId = MqttSensorId.Create(sensorCreatedDomainEvent.DetailId);
                        var hostId = HostId.Create(sensorCreatedDomainEvent.HostId);
                        var newMqttSensor = dbContext.ChangeTracker.Entries<MqttSensor>()
                            .FirstOrDefault(a => a.Entity.Id == mqttSensorId)?.Entity;
                        var hostOfMqttSensor = dbContext.ChangeTracker.Entries<Host>()
                            .FirstOrDefault(a => a.Entity.Id == hostId)?.Entity;
                        var hostName = hostOfMqttSensor?.Uris.FirstOrDefault()?.Value;
                        var isSubscribedBefore = dataMedicContext.Set<MqttSensor>()
                            .Any(a => a.TopicName == newMqttSensor!.TopicName);
                        if (!isSubscribedBefore && hostName != null)
                        {
                            var mqttTopicName = newMqttSensor!.TopicName.Value;
                            _backgroundJobClient.Enqueue(() =>
                                _handleMqttSensorCreatedDomainEvent.Handle(sensorCreatedDomainEvent.SenosorId,
                                    mqttTopicName,
                                    sensorCreatedDomainEvent.HostId,
                                    newMqttSensor.TimeToLiveInSeconds,
                                    cancellationToken)
                            );
                        }
                    }
                    break;
                case SensorDeletedDomainEvent sensorDeletedDomainEvent:
                    var sensorIdFilter = sensorDeletedDomainEvent.SenosorId.ToString();
                    var jobsToRemove = JobStorage.Current.GetMonitoringApi()
                        .EnqueuedJobs("default", 0, int.MaxValue)
                        .Where(x => x.Value.Job is { Args: { } } && x.Value.Job.Args.Any(arg => arg.ToString()!.Contains(sensorIdFilter)))
                        .Select(x => x.Key)
                        .ToList();
                    var processingJobs = JobStorage.Current.GetMonitoringApi().ProcessingJobs(0, int.MaxValue)
                        .Where(x => x.Value.Job is { Args: { } } && x.Value.Job.Args.Any(arg => arg.ToString()!.Contains(sensorIdFilter)))
                        .Select(x => x.Key)
                        .ToList();
                    foreach (var jobIdToDelete in jobsToRemove)
                    {
                        BackgroundJob.Delete(jobIdToDelete);
                    }

                    foreach (var jobIdToDelete in processingJobs)
                    {
                        BackgroundJob.Delete(jobIdToDelete);
                    }

                    RecurringJob.RemoveIfExists(sensorIdFilter);
                    break;
                case SensorStatusUpdatedDomainEvent sensorStatusUpdatedDomainEvent:
                    var handleSensorStatusUpdatedDomainEvent =
                        scope.ServiceProvider.GetRequiredService<IHandleSensorStatusUpdatedDomainEvent>();
                    handleSensorStatusUpdatedDomainEvent.Handle(sensorStatusUpdatedDomainEvent.SenosorId,
                        sensorStatusUpdatedDomainEvent.Status);
                    break;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private string GetCronExpression(TimeSpan timeSpan)
    {
        int seconds = timeSpan.Seconds;
        int minutes = timeSpan.Minutes;
        int hours = timeSpan.Hours;
        int days = timeSpan.Days;
        string cronExpression = $"*/{seconds} * * * * *";
        // Convert TimeSpan to cron expression format
        if (minutes > 0)
        {
            cronExpression = $"*/{minutes} * * * *";
        }
        return cronExpression;
    }
}