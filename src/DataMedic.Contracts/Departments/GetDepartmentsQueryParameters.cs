using DataMedic.Contracts.Common;

namespace DataMedic.Contracts.Departments;

public class GetDepartmentsQueryParameters : PaginationQueryParameters
{
    public string SearchString { get; set; } = string.Empty;
}