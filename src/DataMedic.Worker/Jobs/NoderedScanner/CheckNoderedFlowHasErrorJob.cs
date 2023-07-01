using System;
using System.Net.Http;

using DataMedic.Worker.Jobs.NoderedScanner.Models;
using DataMedic.Worker.Models;

using Hangfire;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using StackExchange.Redis;

namespace DataMedic.Worker.Jobs.NoderedScanner;

public class CheckNoderedFlowHasErrorJob : IHostedService
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly HttpClient _httpClient;
    private readonly List<NoderedFlowDetails> _flowDetailsList;
    private readonly IDatabase _cache;

    public CheckNoderedFlowHasErrorJob(IBackgroundJobClient backgroundJobClient, HttpClient httpClient, IServiceScopeFactory serviceScopeFactory, IDatabase cache)
    {
        _backgroundJobClient = backgroundJobClient;
        _httpClient = httpClient;
        _serviceScopeFactory = serviceScopeFactory;
        _cache = cache;
        _flowDetailsList = GetDetails();
    }

    private List<NoderedFlowDetails> GetDetails()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var noderedScanService = scope.ServiceProvider.GetRequiredService<INoderedScanService>();
        return noderedScanService.GetFlowsWithNoderedHosts();
    }
    [Obsolete("Obsolete")]
    public Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var noderedScanService = scope.ServiceProvider.GetRequiredService<INoderedScanService>();
        var flowsToScan = noderedScanService.GetFlowsWithNoderedHosts();
        foreach (var flow in flowsToScan)
        {
            var sensorId = flow.SensorId.Value;
            RecurringJob.AddOrUpdate(() => ScanFlowErrors(flow,sensorId), "*/50 * * * * *");
        }
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        RecurringJob.RemoveIfExists(nameof(CheckNoderedFlowHasErrorJob.ScanFlowErrors));
        return Task.CompletedTask;
    }

    public async Task ScanFlowErrors(NoderedFlowDetails flow, Guid sensorId)
    {
            var url = "" + flow.NoderedHost + "/api/getErrorList/" + flow.FlowId;
            var response = _httpClient.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<NoderedErrorResponseModel>(responseContent);
            _cache.HashSet("nodered", sensorId.ToString(), responseContent);
    }
}