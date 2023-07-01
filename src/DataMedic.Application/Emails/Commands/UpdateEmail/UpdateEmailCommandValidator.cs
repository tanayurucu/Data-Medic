using FluentValidation;

using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Emails.ValueObjects;

namespace DataMedic.Application.Emails.Commands.UpdateEmail;

public sealed class UpdateEmailCommandValidator : AbstractValidator<UpdateEmailCommand>
{
    public UpdateEmailCommandValidator()
    {
        RuleFor(x => x.EmailId).NotEmpty().WithError(Errors.Email.IdRequired);

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithError(Errors.Email.Address.Empty)
            .MaximumLength(EmailAddress.MaxLength)
            .WithError(Errors.Email.Address.TooLong)
            .EmailAddress()
            .WithError(Errors.Email.Address.Invalid);
    }
}
