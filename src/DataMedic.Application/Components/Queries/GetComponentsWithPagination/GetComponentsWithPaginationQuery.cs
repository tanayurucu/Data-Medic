using DataMedic.Application.Common.Messages;
using DataMedic.Application.Common.Models;
using DataMedic.Domain.Components;

using ErrorOr;

namespace DataMedic.Application.Components.Queries.GetComponentsWithPagination;

public sealed record GetComponentsWithPaginationQuery(
    string SearchString,
    int PageSize,
    int PageNumber
) : IQuery<ErrorOr<Paged<Component>>>;