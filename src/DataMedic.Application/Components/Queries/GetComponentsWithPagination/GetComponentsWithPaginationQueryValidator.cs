using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

using FluentValidation;

namespace DataMedic.Application.Components.Queries.GetComponentsWithPagination;

public sealed class GetComponentsWithPaginationQueryValidator
    : AbstractValidator<GetComponentsWithPaginationQuery>
{
    public GetComponentsWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithError(Errors.Common.Pagination.InvalidPageNumber);

        RuleFor(x => x.PageSize).GreaterThan(0).WithError(Errors.Common.Pagination.InvalidPageSize);
    }
}