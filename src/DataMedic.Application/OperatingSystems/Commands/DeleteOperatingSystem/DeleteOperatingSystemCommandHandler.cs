using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.OperatingSystems;
using DataMedic.Domain.OperatingSystems.ValueObjects;

using ErrorOr;

using OperatingSystem = DataMedic.Domain.OperatingSystems.OperatingSystem;

namespace DataMedic.Application.OperatingSystems.Commands.DeleteOperatingSystem;

public sealed class DeleteOperatingSystemCommandHandler : ICommandHandler<DeleteOperatingSystemCommand, ErrorOr<Deleted>>
{
    private readonly IOperatingSystemRepository _operatingSystemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteOperatingSystemCommandHandler(IOperatingSystemRepository operatingSystemRepository, IUnitOfWork unitOfWork)
    {
        _operatingSystemRepository = operatingSystemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteOperatingSystemCommand request, CancellationToken cancellationToken)
    {
        var operatingSystemId = OperatingSystemId.Create(request.OperatingSystemId);
        if (await _operatingSystemRepository.FindByIdAsync(operatingSystemId, cancellationToken) is not OperatingSystem operatingSystem)
        {
            return Errors.OperatingSystem.NotFound;
        }

        _operatingSystemRepository.Remove(operatingSystem);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }
}