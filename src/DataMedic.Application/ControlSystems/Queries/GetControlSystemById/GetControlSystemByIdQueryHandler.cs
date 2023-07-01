using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.ControlSystems;
using DataMedic.Domain.ControlSystems.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.ControlSystems.Queries.GetControlSystemById;

internal sealed class GetControlSystemByIdQueryHandler : IQueryHandler<GetControlSystemByIdQuery, ErrorOr<ControlSystem>>
{
    private readonly IControlSystemRepository _controlSystemRepository;

    public GetControlSystemByIdQueryHandler(IControlSystemRepository controlSystemRepository)
    {
        _controlSystemRepository = controlSystemRepository;
    }

    public async Task<ErrorOr<ControlSystem>> Handle(GetControlSystemByIdQuery request, CancellationToken cancellationToken)
    {
        var controlSystemId = ControlSystemId.Create(request.ControlSystemId);

        if (await _controlSystemRepository.FindByIdAsync(controlSystemId, cancellationToken) is not ControlSystem controlSystem)
        {
            return Errors.ControlSystem.NotFound;
        }

        return controlSystem;
    }
}