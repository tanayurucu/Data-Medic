using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

using FluentValidation;

namespace DataMedic.Application.ControlSystems.Commands.DeleteControlSystem;

public sealed class DeleteControlSystemCommandValidator
    : AbstractValidator<DeleteControlSystemCommand>
{
    public DeleteControlSystemCommandValidator()
    {
        RuleFor(x => x.ControlSystemId).NotEmpty().WithError(Errors.ControlSystem.IdRequired);
    }
}