using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Application.Common.Models;
using DataMedic.Domain.ControlSystems;

using ErrorOr;

namespace DataMedic.Application.ControlSystems.Queries.GetControlSystemsWithPagination;

internal sealed class GetControlSystemsWithPaginationQueryHandler
    : IQueryHandler<GetControlSystemsWithPaginationQuery, ErrorOr<Paged<ControlSystem>>>
{
    private readonly IControlSystemRepository _controlSystemRepository;

    public GetControlSystemsWithPaginationQueryHandler(
        IControlSystemRepository controlSystemRepository
    )
    {
        _controlSystemRepository = controlSystemRepository;
    }

    public async Task<ErrorOr<Paged<ControlSystem>>> Handle(
        GetControlSystemsWithPaginationQuery request,
        CancellationToken cancellationToken
    ) =>
        await _controlSystemRepository.FindManyWithPaginationAsync(
            request.SearchString,
            request.PageSize,
            request.PageNumber,
            cancellationToken
        );
}