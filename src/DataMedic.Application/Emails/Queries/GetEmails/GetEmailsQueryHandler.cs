using ErrorOr;

using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Application.Common.Models;
using DataMedic.Application.Emails.Models;
using DataMedic.Domain.Departments.ValueObjects;

namespace DataMedic.Application.Emails.Queries.GetEmails;

public sealed class GetEmailsQueryHandler
    : IQueryHandler<GetEmailsQuery, ErrorOr<Paged<EmailWithDepartment>>>
{
    private readonly IEmailRepository _emailRepository;

    public GetEmailsQueryHandler(IEmailRepository emailRepository)
    {
        _emailRepository = emailRepository;
    }

    public async Task<ErrorOr<Paged<EmailWithDepartment>>> Handle(
        GetEmailsQuery request,
        CancellationToken cancellationToken
    )
    {
        var departmentId = DepartmentId.Create(request.DepartmentId);
        return await _emailRepository.FindManyWithPaginationAsync(
            request.SearchString,
            request.PageNumber,
            request.PageSize,
            departmentId,
            cancellationToken
        );
    }
}
