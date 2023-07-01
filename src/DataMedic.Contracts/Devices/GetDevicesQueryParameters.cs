using DataMedic.Contracts.Common;

namespace DataMedic.Contracts.Devices;

public class GetDevicesQueryParameters : PaginationQueryParameters
{
    public string SearchString { get; set; } = string.Empty;
    public Guid DepartmentId { get; set; }
    public Guid DeviceGroupId { get; set; }
}
