using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

using FluentValidation;

namespace DataMedic.Application.Departments.Queries.GetDepartmentsWithPagination;

public sealed class GetDepartmentsWithPaginationQueryValidator : AbstractValidator<GetDepartmentsWithPaginationQuery>
{
    public GetDepartmentsWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithError(Errors.Common.Pagination.InvalidPageNumber);

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithError(Errors.Common.Pagination.InvalidPageSize);
    }
}