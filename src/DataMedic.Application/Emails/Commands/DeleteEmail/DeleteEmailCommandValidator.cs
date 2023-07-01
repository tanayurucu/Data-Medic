using FluentValidation;

using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

namespace DataMedic.Application.Emails.Commands.DeleteEmail;

public sealed class DeleteEmailCommandValidator : AbstractValidator<DeleteEmailCommand>
{
    public DeleteEmailCommandValidator()
    {
        RuleFor(x => x.EmailId).NotEmpty().WithError(Errors.Email.IdRequired);
    }
}
