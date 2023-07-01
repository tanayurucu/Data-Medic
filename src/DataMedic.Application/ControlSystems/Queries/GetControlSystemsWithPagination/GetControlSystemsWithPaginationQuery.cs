using DataMedic.Application.Common.Messages;
using DataMedic.Application.Common.Models;
using DataMedic.Domain.ControlSystems;

using ErrorOr;

namespace DataMedic.Application.ControlSystems.Queries.GetControlSystemsWithPagination;

public record GetControlSystemsWithPaginationQuery(
    string SearchString,
    int PageSize,
    int PageNumber
) : IQuery<ErrorOr<Paged<ControlSystem>>>;