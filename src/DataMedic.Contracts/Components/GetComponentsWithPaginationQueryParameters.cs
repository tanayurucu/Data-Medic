using DataMedic.Contracts.Common;

namespace DataMedic.Contracts.Components;

public class GetComponentsWithPaginationQueryParameters : PaginationQueryParameters
{
    public string SearchString { get; set; } = string.Empty;
}
