using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

using FluentValidation;

namespace DataMedic.Application.Components.Queries.GetComponentById;

public sealed class GetComponentByIdQueryValidator : AbstractValidator<GetComponentByIdQuery>
{
    public GetComponentByIdQueryValidator()
    {
        RuleFor(x => x.ComponentId).NotEmpty().WithError(Errors.Component.IdRequired);
    }
}