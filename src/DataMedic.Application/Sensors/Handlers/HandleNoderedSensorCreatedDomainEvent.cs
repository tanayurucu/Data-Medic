using DataMedic.Application.Common.Interfaces.Infrastructure;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Sensors.Models;
using DataMedic.Domain.Sensors.ValueObjects;

using Newtonsoft.Json;

using StackExchange.Redis;

using IServiceProvider = DataMedic.Application.Common.Interfaces.Infrastructure.IServiceProvider;

namespace DataMedic.Application.Sensors.Handlers;

public class HandleNoderedSensorCreatedDomainEvent : IHandleNoderedSensorCreatedDomainEvent
{
    private readonly HttpClient _httpClient;
    private readonly IDatabase _cache;
    private readonly ISensorRepository _sensorRepository;
    private readonly IEmailRepository _emailRepository;
    private readonly IServiceProvider _serviceProvider;

    public HandleNoderedSensorCreatedDomainEvent(IDatabase cache, ISensorRepository sensorRepository, IEmailRepository emailRepository, IServiceProvider serviceProvider)
    {
        var handler = new HttpClientHandler();
        handler.ClientCertificateOptions = ClientCertificateOption.Manual;
        handler.ServerCertificateCustomValidationCallback = 
            (httpRequestMessage, cert, cetChain, policyErrors) =>
            {
                return true;
            };
        _httpClient = new HttpClient(handler);
        _cache = cache;
        _sensorRepository = sensorRepository;
        _emailRepository = emailRepository;
        _serviceProvider = serviceProvider;
    }

    public async Task Handle(Guid sensorId, string flowId, string hostName)
    {
        bool status;
        bool cacheStatus;
        var url = "" + hostName + "/api/getErrorList/" + flowId;
        var response = _httpClient.GetAsync(url).Result;
        
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseObj = JsonConvert.DeserializeObject<NoderedErrorResponseModel>(responseContent);
        var cacheValue = _cache.HashGet("nodered", sensorId.ToString());
        if (cacheValue.HasValue)
        {
            NoderedErrorResponseModel? cacheValueObj = JsonConvert.DeserializeObject<NoderedErrorResponseModel>(cacheValue.ToString());
            if (cacheValueObj != null)
            {
                cacheStatus = !cacheValueObj.ErrorList.Any();
            }
            else
            {
                cacheStatus = true;
            }
        }
        else
        {
            cacheStatus = false;
        }
        if (responseObj is { ErrorList.Count: > 0 })
        {
            status = false;
            if (cacheStatus)
            {
                var lastError = responseObj.ErrorList.FirstOrDefault()?.Error;
                await _sensorRepository.UpdateNoderedStatus(sensorId,lastError, status);
                var sensorIdObj = SensorId.Create(sensorId);
                var mailingList = await _emailRepository.GetMailingListForSensorIdAsync(sensorIdObj);
                var emailService = _serviceProvider.CreateEmailService().Value;
                await emailService.SendEmailAsync(
                    mailingList.ConvertAll(email => email.Value),
                    "Data Medic: Nodered Flow has an error",
                    "Nodered Flow: " + flowId + " sensor:" + sensorId + " has some error.",
                    CancellationToken.None);
            }
        }
        else
        {
            status = true;
            if (cacheStatus == false)
            {
                await _sensorRepository.UpdateNoderedStatus(sensorId,"", status);
            }
        }
        _cache.HashSet("nodered", sensorId.ToString(), responseContent);
    }
}

public interface IHandleNoderedSensorCreatedDomainEvent
{
    public Task Handle(Guid sensorId, string flowId, string hostName);
}