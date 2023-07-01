using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

using FluentValidation;

namespace DataMedic.Application.ControlSystems.Queries.GetControlSystemsWithPagination;

public sealed class GetControlSystemsQueryValidator
    : AbstractValidator<GetControlSystemsWithPaginationQuery>
{
    public GetControlSystemsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithError(Errors.Common.Pagination.InvalidPageNumber);

        RuleFor(x => x.PageSize).GreaterThan(0).WithError(Errors.Common.Pagination.InvalidPageSize);
    }
}