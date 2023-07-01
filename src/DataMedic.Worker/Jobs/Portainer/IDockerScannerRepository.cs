using DataMedic.Application.Common.Models.Portainer;
using DataMedic.Application.Sensors.Models;
using DataMedic.Domain.Sensors;
using DataMedic.Domain.Sensors.ValueObjects;
using DataMedic.Worker.Jobs.Portainer.Models;
using Host = DataMedic.Domain.Hosts.Host;

namespace DataMedic.Worker.Jobs.Portainer;

public interface IDockerScannerRepository
{
    public List<JoinedDockerDetail> GetJoinedDockerDetails();
    public void UpdateSensorStatus(SensorId sensorId, bool status, string lastErrors);
    public List<Sensor> GetDockerSensors();
    public Task UpdateManySensorStatusByContainerInfoAsync(List<ContainerInfoFromPortainer> notRunningContainers);
    Task UpdateManySensorStatusToUpByContainerIdListAsync(List<string> containerIdsToUpdateUp);
    public Task<Host?> GetPortainerHost();
}