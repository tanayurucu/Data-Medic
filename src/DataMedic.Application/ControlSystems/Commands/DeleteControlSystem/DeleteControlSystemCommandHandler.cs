using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.ControlSystems;
using DataMedic.Domain.ControlSystems.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.ControlSystems.Commands.DeleteControlSystem;

internal sealed class DeleteControlSystemCommandHandler
    : ICommandHandler<DeleteControlSystemCommand, ErrorOr<Deleted>>
{
    private readonly IControlSystemRepository _controlSystemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteControlSystemCommandHandler(
        IControlSystemRepository controlSystemRepository,
        IUnitOfWork unitOfWork
    )
    {
        _controlSystemRepository = controlSystemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Deleted>> Handle(
        DeleteControlSystemCommand request,
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

        _controlSystemRepository.Remove(controlSystem);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }
}