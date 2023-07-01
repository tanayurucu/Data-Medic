using DataMedic.Application.Common.Models.Portainer;
using DataMedic.Application.Sensors.Models;
using DataMedic.Domain.Sensors;
using DataMedic.Domain.Sensors.Entities;
using DataMedic.Domain.Sensors.ValueObjects;
using DataMedic.Persistence;
using DataMedic.Worker.Jobs.Portainer.Models;
using DataMedic.Domain.Hosts.ValueObjects;

using Host = DataMedic.Domain.Hosts.Host;

namespace DataMedic.Worker.Jobs.Portainer;

public class DockerScannerRepository : IDockerScannerRepository
{
    private readonly DataMedicDbContext _dbContext;
    private readonly ILogger<DockerScannerRepository> _logger;

    public DockerScannerRepository(DataMedicDbContext dbContext, ILogger<DockerScannerRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public List<Sensor> GetDockerSensors()
    {
        return _dbContext.Set<Sensor>().Where(a => a.SensorDetail.Type == SensorType.DOCKER).ToList();
    }

    public Task<Host?> GetPortainerHost()
    {
        return Task.FromResult(_dbContext.Set<Host>().FirstOrDefault(a => a.Type == HostType.DOCKER));
    }

    public async Task UpdateManySensorStatusByContainerInfoAsync(List<ContainerInfoFromPortainer> notRunningContainers)
    {
        var containerIdList = notRunningContainers.Select(a => a.Id).ToList();
        var sensorsToUpdate = (
            from dockerSensor in _dbContext.Set<DockerSensor>()
            where containerIdList.Contains(dockerSensor.ContainerId)
            join sensor in _dbContext.Set<Sensor>() on (Guid)dockerSensor.Id equals sensor.SensorDetail.DetailId
            select new
            {
                _sensor = sensor,
                _dockerSensor = dockerSensor
            }
            ).ToList();
        foreach (var sensor in sensorsToUpdate)
        {
            var newStatusInfo = notRunningContainers.FirstOrDefault(a => a.Id == sensor._dockerSensor.ContainerId);
            sensor._sensor.SetStatus(false);
            sensor._sensor.SetStatusText(newStatusInfo?.State);
        }
        var updatedSensors = sensorsToUpdate.Select(a => a._sensor).ToList();
        if (updatedSensors.Any())
        {
            _dbContext.UpdateRange(updatedSensors);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task UpdateManySensorStatusToUpByContainerIdListAsync(List<string> containerIdsToUpdateUp)
    {
        var sensorsToUpdate = (
            from dockerSensor in _dbContext.Set<DockerSensor>()
            where containerIdsToUpdateUp.Contains(dockerSensor.ContainerId)
            join sensor in _dbContext.Set<Sensor>() on (Guid)dockerSensor.Id equals sensor.SensorDetail.DetailId
            select new
            {
                _sensor = sensor,
                _dockerSensor = dockerSensor
            }
        ).ToList();
        foreach (var sensor in sensorsToUpdate)
        {
            sensor._sensor.SetStatus(true);
            sensor._sensor.SetStatusText("running");
        }
        var updatedSensors = sensorsToUpdate.Select(a => a._sensor).ToList();
        _dbContext.UpdateRange(updatedSensors);
        await _dbContext.SaveChangesAsync();
    }

    public void UpdateSensorStatus(SensorId sensorId, bool status, string lastErrors)
    {
        var sensor = _dbContext.Set<Sensor>().FirstOrDefault(a => a.Id == sensorId);
        if (sensor != null)
        {
            sensor?.SetStatus(status);
            sensor?.SetStatusText(lastErrors);
            _dbContext.SaveChanges();
        }
    }

    public List<JoinedDockerDetail> GetJoinedDockerDetails()
    {
        return (
            from sensor in _dbContext.Set<Sensor>()
            where sensor.SensorDetail.Type == SensorType.DOCKER
            join dockerSensor in _dbContext.Set<DockerSensor>() on sensor.SensorDetail.DetailId equals (Guid)dockerSensor.Id
            select new JoinedDockerDetail
            {
                SensorId = sensor.Id,
                Status = sensor.Status,
                DockerSensor = dockerSensor
            }
        ).ToList();
    }
}