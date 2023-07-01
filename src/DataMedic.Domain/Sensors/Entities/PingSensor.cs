using DataMedic.Domain.Common.Abstractions;
using DataMedic.Domain.Hosts.ValueObjects;
using DataMedic.Domain.Sensors.ValueObjects;

namespace DataMedic.Domain.Sensors.Entities;

public sealed class PingSensor : Entity<PingSensorId>, ISensorDetail
{
    private PingSensor(PingSensorId id, TimeSpan scanPeriod)
        : base(id) => ScanPeriod = scanPeriod;

    public TimeSpan ScanPeriod { get; set; }

    private PingSensor() { }

    public static PingSensor Create(TimeSpan scanPeriod) =>
        new(PingSensorId.CreateUnique(), scanPeriod);

    public void SetScanPeriod(TimeSpan scanPeriod) => ScanPeriod = scanPeriod;
}
