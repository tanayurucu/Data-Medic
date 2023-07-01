using DataMedic.Domain.Common.Abstractions;
using DataMedic.Domain.Hosts.ValueObjects;
using DataMedic.Domain.Sensors.ValueObjects;

namespace DataMedic.Domain.Sensors.Entities;

public sealed class NodeRedSensor : Entity<NodeRedSensorId>, ISensorDetail
{
    private NodeRedSensor(NodeRedSensorId id, string flowId, TimeSpan scanPeriodInMinutes)
        : base(id)
    {
        FlowId = flowId;
        ScanPeriodInMinutes = scanPeriodInMinutes;
    }

    public string FlowId { get; private set; }
    public string? LastErrorLog { get; private set; }
    public TimeSpan ScanPeriodInMinutes { get; private set; }

    private NodeRedSensor() { }
    public static NodeRedSensor Create(string flowId, TimeSpan scanPeriodInMinutes) =>
        new(NodeRedSensorId.CreateUnique(), flowId, scanPeriodInMinutes);

    public void SetFlowId(string flowId) => FlowId = flowId;
    public void SetLastErrorLog(string logs) => LastErrorLog = logs;

    public void SetScanPeriodInMinutes(TimeSpan scanPeriodInMinutes) =>
        ScanPeriodInMinutes = scanPeriodInMinutes;
}
