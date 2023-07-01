using ErrorOr;

using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Emails;
using DataMedic.Domain.Emails.ValueObjects;

namespace DataMedic.Application.Emails.Queries.GetEmailById;

public class GetEmailByIdQueryHandler : IQueryHandler<GetEmailByIdQuery, ErrorOr<Email>>
{
    private readonly IEmailRepository _emailRepository;

    public GetEmailByIdQueryHandler(IEmailRepository emailRepository)
    {
        _emailRepository = emailRepository;
    }

    public async Task<ErrorOr<Email>> Handle(
        GetEmailByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var emailId = EmailId.Create(request.EmailId);
        if (await _emailRepository.FindByIdAsync(emailId, cancellationToken) is not Email email)
        {
            return Errors.Email.NotFound;
        }

        return email;
    }
}
