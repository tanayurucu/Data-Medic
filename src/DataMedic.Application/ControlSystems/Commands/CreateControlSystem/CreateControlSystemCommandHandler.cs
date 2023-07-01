using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.ControlSystems;
using DataMedic.Domain.ControlSystems.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.ControlSystems.Commands.CreateControlSystem;

public sealed class CreateControlSystemCommandHandler
    : ICommandHandler<CreateControlSystemCommand, ErrorOr<ControlSystem>>
{
    private readonly IControlSystemRepository _controlSystemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateControlSystemCommandHandler(
        IUnitOfWork unitOfWork,
        IControlSystemRepository controlSystemRepository
    )
    {
        _unitOfWork = unitOfWork;
        _controlSystemRepository = controlSystemRepository;
    }

    public async Task<ErrorOr<ControlSystem>> Handle(
        CreateControlSystemCommand request,
        CancellationToken cancellationToken
    )
    {
        var name = ControlSystemName.Create(request.Name);
        if (
            await _controlSystemRepository.FindByNameAsync(name.Value, cancellationToken)
            is not null
        )
        {
            return Errors.ControlSystem.Name.AlreadyExists(name.Value);
        }

        var errors = Errors.Combine(name);

        if (errors.Any())
        {
            return errors;
        }

        var controlSystem = ControlSystem.Create(name.Value);

        await _controlSystemRepository.AddAsync(controlSystem, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return controlSystem;
    }
}