using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.ControlSystems.ValueObjects;

using ErrorOr;

using FluentValidation;

namespace DataMedic.Application.ControlSystems.Commands.UpdateControlSystem;

public sealed class UpdateControlSystemValidator : AbstractValidator<UpdateControlSystemCommand>
{
    public UpdateControlSystemValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithError(Errors.ControlSystem.Name.Empty);
    }
}