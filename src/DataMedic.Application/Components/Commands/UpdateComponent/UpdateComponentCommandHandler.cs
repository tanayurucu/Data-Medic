using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Components;
using DataMedic.Domain.Components.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.Components.Commands.UpdateComponent;

public sealed class UpdateComponentCommandHandler
    : ICommandHandler<UpdateComponentCommand, ErrorOr<Updated>>
{
    private readonly IComponentRepository _componentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateComponentCommandHandler(
        IComponentRepository componentRepository,
        IUnitOfWork unitOfWork
    )
    {
        _componentRepository = componentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Updated>> Handle(
        UpdateComponentCommand request,
        CancellationToken cancellationToken
    )
    {
        var componentId = ComponentId.Create(request.ComponentId);
        var createComponentNameResult = ComponentName.Create(request.Name);
        if (createComponentNameResult.IsError)
        {
            return createComponentNameResult.Errors;
        }

        if (
            await _componentRepository.FindByIdAsync(componentId, cancellationToken)
            is not Component component
        )
        {
            return Errors.Component.NotFound;
        }

        if (
            await _componentRepository.FindByNameAsync(
                createComponentNameResult.Value,
                cancellationToken
            )
                is Component existingComponent
            && existingComponent.Id != component.Id
        )
        {
            return Errors.Component.Name.AlreadyExists(createComponentNameResult.Value);
        }

        component.SetName(createComponentNameResult.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Updated;
    }
}