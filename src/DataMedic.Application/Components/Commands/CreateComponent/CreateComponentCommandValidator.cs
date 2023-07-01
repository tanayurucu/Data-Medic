using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Components.ValueObjects;

using FluentValidation;

namespace DataMedic.Application.Components.Commands.CreateComponent;

public sealed class CreateComponentCommandValidator : AbstractValidator<CreateComponentCommand>
{
    public CreateComponentCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithError(Errors.Component.Name.Empty)
            .MaximumLength(ComponentName.MaxLength)
            .WithError(Errors.Component.Name.TooLong);
    }
}