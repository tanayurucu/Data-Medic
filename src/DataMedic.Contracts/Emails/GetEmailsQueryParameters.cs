using DataMedic.Contracts.Common;

namespace DataMedic.Contracts.Emails;

public class GetEmailsQueryParameters : PaginationQueryParameters
{
    public string SearchString { get; set; } = string.Empty;
    public Guid DepartmentId { get; set; }
}