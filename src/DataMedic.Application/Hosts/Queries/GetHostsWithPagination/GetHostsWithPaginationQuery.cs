using DataMedic.Application.Common.Messages;
using DataMedic.Application.Common.Models;
using DataMedic.Domain.Hosts;

using ErrorOr;

namespace DataMedic.Application.Hosts.Queries.GetHostsWithPagination;

public sealed record GetHostsWithPaginationQuery(
    int PageSize,
    int PageNumber,
    string SearchString,
    int? HostType
) : IQuery<ErrorOr<Paged<Host>>>;
