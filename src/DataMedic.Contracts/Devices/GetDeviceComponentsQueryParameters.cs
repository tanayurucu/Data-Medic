using DataMedic.Contracts.Common;

namespace DataMedic.Contracts.Devices;

public class GetDeviceComponentsQueryParameters : PaginationQueryParameters
{
    public string SearchString { get; set; } = string.Empty;
    public Guid OperatingSystemId { get; set; }
    public Guid ControlSystemId { get; set; }
}
