using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Application.Common.Models;
using DataMedic.Domain.Components;

using ErrorOr;

namespace DataMedic.Application.Components.Queries.GetComponentsWithPagination;

public sealed class GetComponentsWithPaginationQueryHandler
    : IQueryHandler<GetComponentsWithPaginationQuery, ErrorOr<Paged<Component>>>
{
    private readonly IComponentRepository _componentRepository;

    public GetComponentsWithPaginationQueryHandler(IComponentRepository componentRepository)
    {
        _componentRepository = componentRepository;
    }

    public async Task<ErrorOr<Paged<Component>>> Handle(
        GetComponentsWithPaginationQuery request,
        CancellationToken cancellationToken
    ) =>
        await _componentRepository.FindManyWithPaginationAsync(
            request.SearchString,
            request.PageSize,
            request.PageNumber,
            cancellationToken
        );
}