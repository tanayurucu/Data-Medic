using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Hosts;
using DataMedic.Domain.Hosts.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.Hosts.Queries.GetHostById;

public sealed class GetHostByIdQueryHandler : IQueryHandler<GetHostByIdQuery, ErrorOr<Host>>
{
    private readonly IHostRepository _hostRepository;

    public GetHostByIdQueryHandler(IHostRepository hostRepository)
    {
        _hostRepository = hostRepository;
    }

    public async Task<ErrorOr<Host>> Handle(
        GetHostByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var hostId = HostId.Create(request.HostId);
        if (await _hostRepository.FindByIdAsync(hostId, cancellationToken) is not Host host)
        {
            return Errors.Host.NotFoundWithHostId(hostId);
        }

        return host;
    }
}
