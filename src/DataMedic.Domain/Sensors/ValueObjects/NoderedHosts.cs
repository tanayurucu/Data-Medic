using DataMedic.Domain.Common.Abstractions;

namespace DataMedic.Domain.Sensors.ValueObjects;

public sealed class NoderedHosts : Enumeration<NoderedHosts>
{
    public static readonly NoderedHosts COM_TEST_1 = new(0, "");
    public static readonly NoderedHosts BU1_P4_81 = new(1, "");
    
    private NoderedHosts(int value, string name)
        : base(value, name) { }

    private NoderedHosts() { }
}