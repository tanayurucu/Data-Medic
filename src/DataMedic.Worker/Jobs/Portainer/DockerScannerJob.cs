using System.Diagnostics.CodeAnalysis;

using DataMedic.Application.Common.Interfaces.Infrastructure;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Models.Portainer;

using Hangfire;

using Microsoft.IdentityModel.Tokens;

using StackExchange.Redis;

using Host = DataMedic.Domain.Hosts.Host;
using IServiceProvider = DataMedic.Application.Common.Interfaces.Infrastructure.IServiceProvider;

namespace DataMedic.Worker.Jobs.Portainer;

public class DockerScannerJob : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IDatabase _cache;
    private readonly ILogger<DockerScannerJob> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IEmailService _emailService;

    public DockerScannerJob(IServiceScopeFactory serviceScopeFactory, IDatabase cache, ILogger<DockerScannerJob> logger,
        IServiceProvider serviceProvider, IEmailService emailService)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _cache = cache;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _emailService = emailService;
    }
    [Obsolete("Obsolete")]
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dockerScannerRepository = scope.ServiceProvider.GetRequiredService<IDockerScannerRepository>();
            var portainerHost = await dockerScannerRepository.GetPortainerHost();
            _logger.LogWarning("docker service started!!!");
            if (portainerHost != null)
            {
                var portainerService = _serviceProvider.CreatePortainerService(portainerHost);
                if (portainerService.IsError)
                {
                    _logger.LogError(portainerService.Errors.ToString());
                    return;
                }
                var hostList = await portainerService.Value.GetHostsAsync(cancellationToken);
                if (hostList.IsError)
                {
                    _logger.LogError(hostList.Errors.ToString());
                }
                else
                {
                    if (!hostList.Value.Any())
                    {
                        _logger.LogError("docker host not found!");
                    }
                    foreach (var host in hostList.Value)
                    {
                        _logger.LogWarning("Docker scanner started for: " + host.Id);
                        RecurringJob.AddOrUpdate(host.Name, () => ScanContainerStatus(host.Id,host.Name), "*/50 * * * * *");
                    }
                }
            }
            else
            {
                _logger.LogError("Portainer Host Not Found!");
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
        }
    }

    [SuppressMessage("ReSharper.DPA", "DPA0009: High execution time of DB command", MessageId = "time: 49327ms")]
    public async Task ScanContainerStatus(long id, string name)
    {
        try
        {
            var host = new PortainerHostInformation(id, name);
            using var scope = _serviceScopeFactory.CreateScope();
            var dockerScannerRepository = scope.ServiceProvider.GetRequiredService<IDockerScannerRepository>();
            var portainerHost = await dockerScannerRepository.GetPortainerHost();
            var portainerService = _serviceProvider.CreatePortainerService(portainerHost).Value;
            var containerStatuses = await portainerService.FindDockerContainerStatusListAsync(host.Id);
            var notRunningContainers = containerStatuses.Where(a => a.State != "running").ToList();
            var downContainerIdList = notRunningContainers.Select(a => a.Id).ToList();
            var hashName = "downContainers_" + host.Id;
            var preDownContainers = _cache.HashGet("docker", hashName).ToString();
            if (preDownContainers.IsNullOrEmpty() && !downContainerIdList.Any())
            {
                // no any down container and no any to update UP
                return;
            }
            var preDownContainersList = preDownContainers.Split(',').ToList();
            var containerIdsToUpdateUp = preDownContainersList.Except(downContainerIdList).ToList();
            var containersToUpdateDown =
                notRunningContainers.Where(a => !preDownContainersList.Contains(a.Id)).ToList();
            if (containersToUpdateDown.Any())
            {
                var downContainerIdListStr = string.Join(",", downContainerIdList);
                _logger.LogInformation("downContainerIdListStr" + " Updated to status down");
                _cache.HashSet("docker", hashName, downContainerIdListStr);
                await dockerScannerRepository.UpdateManySensorStatusByContainerInfoAsync(containersToUpdateDown);
                var emailRepository = scope.ServiceProvider.GetRequiredService<IEmailRepository>();
                var mailingList = await emailRepository.FindAllAsync();
                var containerIdOfDown = containersToUpdateDown[0].Names[0];
                await _emailService.SendEmailAsync(
                    mailingList.ConvertAll(email => email.Address.Value),
                    "Data Medic: Docker Container is down.",
                    "container: " + containerIdOfDown + " at portainer Id:" + host.Id + " is not running.",
                    CancellationToken.None);
            }

            if (containerIdsToUpdateUp.Any())
            {
                await dockerScannerRepository.UpdateManySensorStatusToUpByContainerIdListAsync(containerIdsToUpdateUp);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        RecurringJob.RemoveIfExists(nameof(DockerScannerJob.ScanContainerStatus));
        return Task.CompletedTask;
    }
}