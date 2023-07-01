using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Components;
using DataMedic.Domain.Components.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.Components.Queries.GetComponentById;

public sealed class GetComponentByIdQueryHandler
    : IQueryHandler<GetComponentByIdQuery, ErrorOr<Component>>
{
    private readonly IComponentRepository _componentRepository;

    public GetComponentByIdQueryHandler(IComponentRepository componentRepository)
    {
        _componentRepository = componentRepository;
    }

    public async Task<ErrorOr<Component>> Handle(
        GetComponentByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var componentId = ComponentId.Create(request.ComponentId);

        if (
            await _componentRepository.FindByIdAsync(componentId, cancellationToken)
            is not Component component
        )
        {
            return Errors.Component.NotFound;
        }

        return component;
    }
}