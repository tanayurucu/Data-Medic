using DataMedic.Contracts.Common;

namespace DataMedic.Contracts.ControlSystems;

public class GetControlSystemsQueryParameters : PaginationQueryParameters
{
    public string SearchString { get; set; } = string.Empty;
}