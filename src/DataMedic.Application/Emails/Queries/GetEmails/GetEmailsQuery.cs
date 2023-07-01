using DataMedic.Application.Common.Messages;

using ErrorOr;

using DataMedic.Application.Common.Models;
using DataMedic.Application.Emails.Models;

namespace DataMedic.Application.Emails.Queries.GetEmails;

public sealed record GetEmailsQuery(
    string SearchString,
    int PageSize,
    int PageNumber,
    Guid DepartmentId
) : IQuery<ErrorOr<Paged<EmailWithDepartment>>>;
