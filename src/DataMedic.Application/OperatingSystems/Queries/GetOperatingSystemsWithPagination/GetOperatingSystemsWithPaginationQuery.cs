using DataMedic.Application.Common.Messages;
using DataMedic.Application.Common.Models;

using ErrorOr;

using OperatingSystem = DataMedic.Domain.OperatingSystems.OperatingSystem;

namespace DataMedic.Application.OperatingSystems.Queries.GetOperatingSystemsWithPagination;

public record GetOperatingSystemsWithPaginationQuery(
    string SearchString,
    int PageSize,
    int PageNumber
) : IQuery<ErrorOr<Paged<OperatingSystem>>>;
