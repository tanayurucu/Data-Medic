using FluentValidation;

namespace DataMedic.Application.Components.Queries.GetAllComponents;

public sealed class GetAllComponentsQueryValidator : AbstractValidator<GetAllComponentsQuery>
{
    public GetAllComponentsQueryValidator() { }
}