using DataMedic.Domain.Sensors;
using DataMedic.Domain.Sensors.Entities;
using DataMedic.Domain.Sensors.ValueObjects;
using DataMedic.Persistence;
using DataMedic.Worker.Jobs.NoderedScanner.Models;
using Host = DataMedic.Domain.Hosts.Host;

namespace DataMedic.Worker.Jobs.NoderedScanner;

public class NoderedScanService : INoderedScanService
{
    private readonly DataMedicDbContext _dbContext;
    private readonly ILogger<NoderedScanService> _logger;

    public NoderedScanService(DataMedicDbContext dbContext, ILogger<NoderedScanService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public List<NoderedFlowDetails> GetFlowsWithNoderedHosts()
    {
        return 
            (from sensor in _dbContext.Set<Sensor>()
            where sensor.SensorDetail.Type == SensorType.NODE_RED
            join noderedSensor in _dbContext.Set<NodeRedSensor>() on sensor.SensorDetail.DetailId equals (Guid)noderedSensor.Id
            join host in _dbContext.Set<Host>() on sensor.HostId equals host.Id
            select new NoderedFlowDetails { NoderedHost = host.Uris.FirstOrDefault().Value
                , FlowId = noderedSensor.FlowId
                , Status = sensor.Status
                , SensorId = sensor.Id
            }
            ).ToList();
    }

    public List<Sensor> GetNoderedSensors()
    {
        return _dbContext.Set<Sensor>().Where(a => a.SensorDetail.Type == SensorType.NODE_RED).ToList();
    }

    public void UpdateSensorStatus(SensorId sensorId, bool status)
    {
        var sensor = _dbContext.Set<Sensor>().FirstOrDefault(a => a.Id == sensorId);
        sensor?.SetStatus(status);
        _dbContext.SaveChanges();
    }
}