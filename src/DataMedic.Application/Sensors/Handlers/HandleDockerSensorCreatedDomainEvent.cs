using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Domain.Hosts.ValueObjects;
using DataMedic.Domain.Sensors.ValueObjects;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using StackExchange.Redis;

using IServiceProvider = DataMedic.Application.Common.Interfaces.Infrastructure.IServiceProvider;

namespace DataMedic.Application.Sensors.Handlers;

public class HandleDockerSensorCreatedDomainEvent : IHandleDockerSensorCreatedDomainEvent
{
    private readonly IDatabase _cache;
    private readonly ISensorRepository _sensorRepository;
    private readonly IHostRepository _hostRepository;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<HandleDockerSensorCreatedDomainEvent> _logger;
    private readonly IEmailRepository _emailRepository;

    public HandleDockerSensorCreatedDomainEvent(IDatabase cache, ISensorRepository sensorRepository, IServiceProvider serviceProvider, ILogger<HandleDockerSensorCreatedDomainEvent> logger, IHostRepository hostRepository, IEmailRepository emailRepository)
    {
        _cache = cache;
        _sensorRepository = sensorRepository;
        _serviceProvider = serviceProvider;
        _logger = logger;
        _hostRepository = hostRepository;
        _emailRepository = emailRepository;
    }

    public async Task Handle(Guid sensorId, int portainerId, string containerId, Guid hostIdGuid)
    {
        var hostId = HostId.Create(hostIdGuid);
        var host = await _hostRepository.FindByIdAsync(hostId);
        if (host == null)
        {
            _logger.LogError("host not found");
            return;
        }
        var portainerService = _serviceProvider.CreatePortainerService(host);
        if (portainerService.IsError)
        {
            _logger.LogError(portainerService.Errors.ToString());
            return;
        }
        bool cacheStatus;
        bool status;
        var cacheValue = _cache.HashGet("docker", sensorId.ToString());
        if (cacheValue.HasValue)
        {
            cacheStatus = JsonConvert.DeserializeObject<bool>(cacheValue!);
        }
        else
        {
            cacheStatus = false;
        }
        var containerDetail = await portainerService.Value.FindContainerLogsAsync(portainerId,
            containerId);
        if (containerDetail.Contains("ERROR"))
        {
            status = false;
            if (cacheStatus)
            {
                await _sensorRepository.UpdateDockerStatus(sensorId,status, containerDetail);
                var sensorIdObj = SensorId.Create(sensorId);
                var mailingList = await _emailRepository.GetMailingListForSensorIdAsync(sensorIdObj);
                var emailService = _serviceProvider.CreateEmailService().Value;
                await emailService.SendEmailAsync(
                    mailingList.ConvertAll(email => email.Value),
                    "Data Medic: Docker container has an error",
                    "Docker Container: " + containerId + " at host:" + portainerId + " has some error.",
                    CancellationToken.None);
            }
        }
        else
        {
            status = true;
            if (cacheStatus == false)
            {
                await _sensorRepository.UpdateDockerStatus(sensorId,status, containerDetail);
            }
        }
        _cache.HashSet("docker", sensorId.ToString(), status);
    }
}

public interface IHandleDockerSensorCreatedDomainEvent
{
    public Task Handle(Guid sensorId, int portainerId, string containerId, Guid hostIdGuid);
}