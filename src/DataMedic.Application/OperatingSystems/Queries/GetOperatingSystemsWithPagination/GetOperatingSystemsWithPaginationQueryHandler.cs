using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Application.Common.Models;

using ErrorOr;

using OperatingSystem = DataMedic.Domain.OperatingSystems.OperatingSystem;

namespace DataMedic.Application.OperatingSystems.Queries.GetOperatingSystemsWithPagination;

internal sealed class GetOperatingSystemsWithPaginationQueryHandler
    : IQueryHandler<GetOperatingSystemsWithPaginationQuery, ErrorOr<Paged<OperatingSystem>>>
{
    private readonly IOperatingSystemRepository _operatingSystemRepository;

    public GetOperatingSystemsWithPaginationQueryHandler(
        IOperatingSystemRepository operatingSystemRepository
    )
    {
        _operatingSystemRepository = operatingSystemRepository;
    }

    public async Task<ErrorOr<Paged<OperatingSystem>>> Handle(
        GetOperatingSystemsWithPaginationQuery request,
        CancellationToken cancellationToken
    ) =>
        await _operatingSystemRepository.FindManyWithPaginationAsync(
            request.SearchString,
            request.PageSize,
            request.PageNumber,
            cancellationToken
        );
}
