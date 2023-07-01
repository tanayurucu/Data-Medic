using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

using FluentValidation;

namespace DataMedic.Application.Components.Commands.DeleteComponent;

public sealed class DeleteComponentCommandValidator : AbstractValidator<DeleteComponentCommand>
{
    public DeleteComponentCommandValidator()
    {
        RuleFor(x => x.ComponentId).NotEmpty().WithError(Errors.Component.IdRequired);
    }
}