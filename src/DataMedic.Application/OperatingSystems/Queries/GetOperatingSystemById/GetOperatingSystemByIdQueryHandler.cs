using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.OperatingSystems;
using DataMedic.Domain.OperatingSystems.ValueObjects;

using ErrorOr;

using OperatingSystem = DataMedic.Domain.OperatingSystems.OperatingSystem;

namespace DataMedic.Application.OperatingSystems.Queries.GetOperatingSystemById;

internal sealed class GetOperatingSystemByIdQueryHandler : IQueryHandler<GetOperatingSystemByIdQuery, ErrorOr<OperatingSystem>>
{
    private readonly IOperatingSystemRepository _operatingSystemRepository;

    public GetOperatingSystemByIdQueryHandler(IOperatingSystemRepository operatingSystemRepository)
    {
        _operatingSystemRepository = operatingSystemRepository;
    }

    public async Task<ErrorOr<OperatingSystem>> Handle(GetOperatingSystemByIdQuery request, CancellationToken cancellationToken)
    {
        var operatingSystemId = OperatingSystemId.Create(request.OperatingSystemId);

        if (await _operatingSystemRepository.FindByIdAsync(operatingSystemId, cancellationToken) is not OperatingSystem operatingSystem)
        {
            return Errors.OperatingSystem.NotFound;
        }

        return operatingSystem;
    }
}