using FluentValidation;

using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Emails.ValueObjects;

namespace DataMedic.Application.Emails.Commands.CreateEmail;

public sealed class CreateEmailCommandValidator : AbstractValidator<CreateEmailCommand>
{
    public CreateEmailCommandValidator()
    {
        RuleFor(x => x.DepartmentId).NotEmpty();

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithError(Errors.Email.Address.Empty)
            .MaximumLength(EmailAddress.MaxLength)
            .WithError(Errors.Email.Address.TooLong)
            .EmailAddress()
            .WithError(Errors.Email.Address.Invalid);
    }
}
