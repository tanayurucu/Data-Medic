using System.Net;

using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Domain.Sensors.ValueObjects;

using Newtonsoft.Json;
using IServiceProvider = DataMedic.Application.Common.Interfaces.Infrastructure.IServiceProvider;
using StackExchange.Redis;

namespace DataMedic.Application.Sensors.Handlers;

public class HandlePingSensorCreatedDomainEvent : IHandlePingSensorCreatedDomainEvent
{
    private readonly HttpClient _httpClient;
    private readonly IDatabase _cache;
    private readonly ISensorRepository _sensorRepository;
    private readonly IEmailRepository _emailRepository;
    private readonly IServiceProvider _serviceProvider;

    public HandlePingSensorCreatedDomainEvent(IDatabase cache, ISensorRepository sensorRepository, IEmailRepository emailRepository, IServiceProvider serviceProvider)
    {
        _cache = cache;
        _sensorRepository = sensorRepository;
        _emailRepository = emailRepository;
        _serviceProvider = serviceProvider;
        _httpClient = new HttpClient();
    }

    public async Task Handle(Guid sensorId,string ipAdress, string hostAddress, TimeSpan scanPeriod)
    {
        _httpClient.BaseAddress = new Uri(hostAddress);
        var response = await _httpClient.GetAsync($"/ping?ip={ipAdress}");
        var cacheStatus = false;
        var cacheValue = _cache.HashGet("ping", ipAdress);
        if (cacheValue.HasValue)
        {
            cacheStatus = JsonConvert.DeserializeObject<bool>(cacheValue!);
        }
        if (response.StatusCode == HttpStatusCode.OK)
        {
            if (cacheStatus == false)
            {
                await _sensorRepository.UpdatePingStatus(sensorId, true);
            }
            _cache.HashSet("ping", ipAdress, true);
        }else if (response.StatusCode == (HttpStatusCode)400)
        {
            Console.WriteLine("Please provide an IP address");
        }
        else
        {
            if (cacheStatus)
            {
                await _sensorRepository.UpdatePingStatus(sensorId, false);
                var sensorIdObj = SensorId.Create(sensorId);
                var mailingList = await _emailRepository.GetMailingListForSensorIdAsync(sensorIdObj);
                var emailService = _serviceProvider.CreateEmailService().Value;
                await emailService.SendEmailAsync(
                    mailingList.ConvertAll(email => email.Value),
                    "Data Medic: Ping sensor has an error",
                    "ip adress: " + ipAdress + " at host:" + hostAddress + " has no ping.",
                    CancellationToken.None);
            }
            _cache.HashSet("ping", ipAdress, false);
        }
    }
}

public interface IHandlePingSensorCreatedDomainEvent
{
    public Task Handle(Guid sensorId, string ipAdress, string hostAddress, TimeSpan scanPeriod);
}