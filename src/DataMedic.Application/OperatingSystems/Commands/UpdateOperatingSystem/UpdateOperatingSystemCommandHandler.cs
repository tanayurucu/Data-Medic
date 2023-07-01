using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.OperatingSystems;
using DataMedic.Domain.OperatingSystems.ValueObjects;

using ErrorOr;

using OperatingSystem = DataMedic.Domain.OperatingSystems.OperatingSystem;

namespace DataMedic.Application.OperatingSystems.Commands.UpdateOperatingSystem;

public sealed class UpdateOperatingSystemCommandHandler : ICommandHandler<UpdateOperatingSystemCommand, ErrorOr<Updated>>
{
    private readonly IOperatingSystemRepository _operatingSystemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOperatingSystemCommandHandler(IOperatingSystemRepository operatingSystemRepository, IUnitOfWork unitOfWork)
    {
        _operatingSystemRepository = operatingSystemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Updated>> Handle(UpdateOperatingSystemCommand request, CancellationToken cancellationToken)
    {
        var operatingSystemId = OperatingSystemId.Create(request.OperatingSystemId);
        if (await _operatingSystemRepository.FindByIdAsync(operatingSystemId, cancellationToken) is not OperatingSystem operatingSystem)
        {
            return Errors.OperatingSystem.NotFound;
        }

        ErrorOr<OperatingSystemName> createOperatingSystemNameResult = OperatingSystemName.Create(request.Name);
        if (createOperatingSystemNameResult.IsError)
        {
            return createOperatingSystemNameResult.Errors;
        }

        if (await _operatingSystemRepository.FindByNameAsync(createOperatingSystemNameResult.Value, cancellationToken) is
                OperatingSystem existingOperatingSystem && existingOperatingSystem.Id != operatingSystemId)
        {
            return Errors.OperatingSystem.Name.AlreadyExists(createOperatingSystemNameResult.Value);
        }

        operatingSystem.SetName(createOperatingSystemNameResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Updated;
    }
}