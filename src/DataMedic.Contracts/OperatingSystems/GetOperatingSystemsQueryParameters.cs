using DataMedic.Contracts.Common;

namespace DataMedic.Contracts.OperatingSystems;

public class GetOperatingSystemsQueryParameters : PaginationQueryParameters
{
    public string SearchString { get; set; } = string.Empty;
}