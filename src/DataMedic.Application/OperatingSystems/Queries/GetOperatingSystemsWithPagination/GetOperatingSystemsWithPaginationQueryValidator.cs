using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

using FluentValidation;

namespace DataMedic.Application.OperatingSystems.Queries.GetOperatingSystemsWithPagination;

public sealed class GetOperatingSystemsWithPaginationQueryValidator
    : AbstractValidator<GetOperatingSystemsWithPaginationQuery>
{
    public GetOperatingSystemsWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithError(Errors.Common.Pagination.InvalidPageNumber);

        RuleFor(x => x.PageSize).GreaterThan(0).WithError(Errors.Common.Pagination.InvalidPageSize);
    }
}
