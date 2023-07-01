using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.ControlSystems;
using DataMedic.Domain.ControlSystems.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.ControlSystems.Commands.UpdateControlSystem;

internal sealed class UpdateControlSystemCommandHandler
    : ICommandHandler<UpdateControlSystemCommand, ErrorOr<Updated>>
{
    private readonly IControlSystemRepository _controlSystemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateControlSystemCommandHandler(
        IControlSystemRepository controlSystemRepository,
        IUnitOfWork unitOfWork
    )
    {
        _controlSystemRepository = controlSystemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Updated>> Handle(
        UpdateControlSystemCommand request,
        CancellationToken cancellationToken
    )
    {
        var controlSystemId = ControlSystemId.Create(request.ControlSystemId);
        if (
            await _controlSystemRepository.FindByIdAsync(controlSystemId, cancellationToken)
            is not ControlSystem controlSystem
        )
        {
            return Errors.ControlSystem.NotFound;
        }

        var nameResult = ControlSystemName.Create(request.Name);
        if (nameResult.IsError)
        {
            return nameResult.Errors;
        }

        if (
            await _controlSystemRepository.FindByNameAsync(nameResult.Value, cancellationToken)
                is ControlSystem existingControlSystem
            && existingControlSystem.Id != controlSystemId
        )
        {
            return Errors.ControlSystem.Name.AlreadyExists(nameResult.Value);
        }

        controlSystem.SetName(nameResult.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Updated;
    }
}