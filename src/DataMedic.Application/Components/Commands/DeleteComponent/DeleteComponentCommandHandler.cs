using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Components;
using DataMedic.Domain.Components.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.Components.Commands.DeleteComponent;

public sealed class DeleteComponentCommandHandler
    : ICommandHandler<DeleteComponentCommand, ErrorOr<Component>>
{
    private readonly IComponentRepository _componentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteComponentCommandHandler(
        IComponentRepository componentRepository,
        IUnitOfWork unitOfWork
    )
    {
        _componentRepository = componentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Component>> Handle(
        DeleteComponentCommand request,
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

        _componentRepository.Remove(component);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return component;
    }
}