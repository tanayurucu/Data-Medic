using DataMedic.Contracts.Common;

namespace DataMedic.Contracts.Hosts;

public class GetHostsWithPaginationQueryParameters : PaginationQueryParameters
{
    public string SearchString { get; set; } = string.Empty;
    public int? HostType { get; set; } = null;
}
