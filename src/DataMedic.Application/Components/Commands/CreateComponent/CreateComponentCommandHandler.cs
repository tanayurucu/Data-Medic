using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Components;
using DataMedic.Domain.Components.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.Components.Commands.CreateComponent;

public sealed class CreateComponentCommandHandler
    : ICommandHandler<CreateComponentCommand, ErrorOr<Component>>
{
    private readonly IComponentRepository _componentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateComponentCommandHandler(
        IComponentRepository componentRepository,
        IUnitOfWork unitOfWork
    )
    {
        _componentRepository = componentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Component>> Handle(
        CreateComponentCommand request,
        CancellationToken cancellationToken
    )
    {
        var createComponentNameResult = ComponentName.Create(request.Name);
        if (createComponentNameResult.IsError)
        {
            return createComponentNameResult.Errors;
        }

        if (
            await _componentRepository.FindByNameAsync(
                createComponentNameResult.Value,
                cancellationToken
            )
            is not null
        )
        {
            return Errors.Component.Name.AlreadyExists(createComponentNameResult.Value);
        }

        var component = Component.Create(createComponentNameResult.Value);

        await _componentRepository.AddAsync(component, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return component;
    }
}