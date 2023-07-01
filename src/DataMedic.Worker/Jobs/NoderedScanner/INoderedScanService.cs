using DataMedic.Domain.Sensors;
using DataMedic.Domain.Sensors.ValueObjects;
using DataMedic.Worker.Jobs.NoderedScanner.Models;

namespace DataMedic.Worker.Jobs.NoderedScanner;

public interface INoderedScanService
{
    public List<NoderedFlowDetails> GetFlowsWithNoderedHosts();
    public List<Sensor> GetNoderedSensors();
    void UpdateSensorStatus(SensorId sensorId, bool status);
}