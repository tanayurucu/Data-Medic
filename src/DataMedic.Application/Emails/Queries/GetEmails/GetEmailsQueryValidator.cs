using FluentValidation;

using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

namespace DataMedic.Application.Emails.Queries.GetEmails;

public sealed class GetEmailsQueryValidator : AbstractValidator<GetEmailsQuery>
{
    public GetEmailsQueryValidator()
    {
    }
}
