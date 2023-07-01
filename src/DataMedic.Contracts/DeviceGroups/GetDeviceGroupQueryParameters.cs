using DataMedic.Contracts.Common;

namespace DataMedic.Contracts.DeviceGroups;

public class GetDeviceGroupQueryParameters : PaginationQueryParameters
{
    public string SearchString { get; set; } = string.Empty;
}