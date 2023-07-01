using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.OperatingSystems;
using DataMedic.Domain.OperatingSystems.ValueObjects;

using ErrorOr;

using OperatingSystem = DataMedic.Domain.OperatingSystems.OperatingSystem;

namespace DataMedic.Application.OperatingSystems.Commands.CreateOperatingSystem;

public sealed class CreateOperatingSystemCommandHandler : ICommandHandler<CreateOperatingSystemCommand, ErrorOr<OperatingSystem>>
{
    private readonly IOperatingSystemRepository _operatingSystemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOperatingSystemCommandHandler(IOperatingSystemRepository operatingSystemRepository, IUnitOfWork unitOfWork)
    {
        _operatingSystemRepository = operatingSystemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<OperatingSystem>> Handle(CreateOperatingSystemCommand request, CancellationToken cancellationToken)
    {
        ErrorOr<OperatingSystemName> createOperatingSystemNameResult = OperatingSystemName.Create(request.Name);
        if (createOperatingSystemNameResult.IsError)
        {
            return createOperatingSystemNameResult.Errors;
        }

        if (await _operatingSystemRepository.FindByNameAsync(createOperatingSystemNameResult.Value,
                cancellationToken) is not null)
        {
            return Errors.OperatingSystem.Name.AlreadyExists(createOperatingSystemNameResult.Value);
        }

        var operatingSystem = OperatingSystem.Create(createOperatingSystemNameResult.Value);
        await _operatingSystemRepository.AddAsync(operatingSystem, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return operatingSystem;
    }
}