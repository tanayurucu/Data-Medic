using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

using FluentValidation;

namespace DataMedic.Application.Devices.Queries.GetDeviceComponentsWithPagination;

public sealed class GetDeviceComponentsWithPaginationQueryValidator
    : AbstractValidator<GetDeviceComponentsWithPaginationQuery>
{
    public GetDeviceComponentsWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithError(Errors.Common.Pagination.InvalidPageNumber);

        RuleFor(x => x.PageSize).GreaterThan(0).WithError(Errors.Common.Pagination.InvalidPageSize);
    }
}
