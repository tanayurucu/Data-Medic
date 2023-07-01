using FluentValidation;

using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

namespace DataMedic.Application.Emails.Queries.GetEmailById;

public sealed class GetEmailByIdQueryValidator : AbstractValidator<GetEmailByIdQuery>
{
    public GetEmailByIdQueryValidator()
    {
        RuleFor(x => x.EmailId).NotEmpty().WithError(Errors.Email.IdRequired);
    }
}
