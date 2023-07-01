using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Components.ValueObjects;

using FluentValidation;

namespace DataMedic.Application.Components.Commands.UpdateComponent;

public sealed class UpdateComponentCommandValidator : AbstractValidator<UpdateComponentCommand>
{
    public UpdateComponentCommandValidator()
    {
        RuleFor(x => x.ComponentId).NotEmpty().WithError(Errors.Component.IdRequired);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithError(Errors.Component.Name.Empty)
            .MaximumLength(ComponentName.MaxLength)
            .WithError(Errors.Component.Name.TooLong);
    }
}