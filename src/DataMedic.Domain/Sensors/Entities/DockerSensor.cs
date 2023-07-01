using DataMedic.Domain.Common.Abstractions;
using DataMedic.Domain.Hosts.ValueObjects;
using DataMedic.Domain.Sensors.ValueObjects;

namespace DataMedic.Domain.Sensors.Entities;

public class DockerSensor : Entity<DockerSensorId>, ISensorDetail
{
    public TimeSpan ScanPeriod { get; private set; }
    public int PortainerId { get; private set; }
    public string ContainerId { get; private set; }
    public string? LastLog { get; private set; }

    private DockerSensor(
        DockerSensorId id,
        int portainerId,
        string containerId,
        TimeSpan scanPeriod
    )
        : base(id)
    {
        PortainerId = portainerId;
        ContainerId = containerId;
        ScanPeriod = scanPeriod;
    }
    public DockerSensor(){}
    public static DockerSensor Create(int portainerId, string containerId, TimeSpan scanPeriod) =>
        new(DockerSensorId.CreateUnique(), portainerId, containerId, scanPeriod);

    public void SetLastLog(string logs) => LastLog = logs;

    public void SetPortainerId(int portainerId) => PortainerId = portainerId;

    public void SetContainerId(string containerId) => ContainerId = containerId;

    public void SetScanPeriod(TimeSpan scanPeriod) => ScanPeriod = scanPeriod;

}

