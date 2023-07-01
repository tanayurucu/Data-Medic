using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

using FluentValidation;

namespace DataMedic.Application.ControlSystems.Commands.CreateControlSystem;

public class CreateControlSystemCommandValidator : AbstractValidator<CreateControlSystemCommand>
{
    public CreateControlSystemCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithError(Errors.ControlSystem.Name.Empty);
    }
}