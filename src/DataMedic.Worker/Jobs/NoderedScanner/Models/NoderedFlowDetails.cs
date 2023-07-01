using DataMedic.Domain.Sensors.ValueObjects;

namespace DataMedic.Worker.Jobs.NoderedScanner.Models;

public class NoderedFlowDetails
{
    public string NoderedHost { get; set; }
    public string FlowId { get; set; }
    public bool Status { get; set; }
    public SensorId SensorId { get; set; }
}