using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Components;

using ErrorOr;

namespace DataMedic.Application.Components.Queries.GetAllComponents;

public sealed class GetAllComponentsQueryHandler
    : IQueryHandler<GetAllComponentsQuery, ErrorOr<List<Component>>>
{
    private readonly IComponentRepository _componentRepository;

    public GetAllComponentsQueryHandler(IComponentRepository componentRepository)
    {
        _componentRepository = componentRepository;
    }

    public async Task<ErrorOr<List<Component>>> Handle(
        GetAllComponentsQuery request,
        CancellationToken cancellationToken
    ) => await _componentRepository.FindAllAsync(cancellationToken);
}