using DataMedic.Domain.Sensors.Entities;
using DataMedic.Domain.Sensors.ValueObjects;

namespace DataMedic.Worker.Jobs.Portainer.Models;

public class JoinedDockerDetail
{
    public SensorId SensorId { get; set; }
    public bool Status { get; set; }
    public DockerSensor DockerSensor { get; set; }
}