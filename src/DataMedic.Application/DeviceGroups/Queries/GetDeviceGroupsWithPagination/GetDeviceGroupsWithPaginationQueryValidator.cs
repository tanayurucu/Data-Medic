using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

using FluentValidation;

namespace DataMedic.Application.DeviceGroups.Queries.GetDeviceGroupsWithPagination;

public sealed class GetDeviceGroupsWithPaginationQueryValidator : AbstractValidator<GetDeviceGroupsWithPaginationQuery>
{
    public GetDeviceGroupsWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithError(Errors.Common.Pagination.InvalidPageNumber);

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithError(Errors.Common.Pagination.InvalidPageSize);
    }
}