using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Hosts.ValueObjects;

using FluentValidation;

namespace DataMedic.Application.Hosts.Queries.GetHostsWithPagination;

public sealed class GetHostsWithPaginationQueryValidator
    : AbstractValidator<GetHostsWithPaginationQuery>
{
    public GetHostsWithPaginationQueryValidator()
    {
        RuleFor(x => x.HostType)
            .Must(value => value is null || Enum.IsDefined(typeof(HostType), value))
            .WithError(Errors.Host.InvalidHostType);

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithError(Errors.Common.Pagination.InvalidPageNumber);

        RuleFor(x => x.PageSize).GreaterThan(0).WithError(Errors.Common.Pagination.InvalidPageSize);
    }
}
