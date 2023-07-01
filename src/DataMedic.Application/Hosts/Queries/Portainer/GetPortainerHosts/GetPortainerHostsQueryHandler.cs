using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Application.Common.Models.Portainer;

using ErrorOr;
using IServiceProvider = DataMedic.Application.Common.Interfaces.Infrastructure.IServiceProvider;
using DataMedic.Domain.Hosts.ValueObjects;
using DataMedic.Domain.Hosts;
using DataMedic.Domain.Common.Errors;

namespace DataMedic.Application.Hosts.Queries.Portainer.GetPortainerHosts;

public sealed class GetPortainerHostsQueryHandler
    : IQueryHandler<GetPortainerHostsQuery, ErrorOr<List<PortainerHostInformation>>>
{
    private readonly IHostRepository _hostRepository;
    private readonly IServiceProvider _serviceProvider;

    public GetPortainerHostsQueryHandler(
        IHostRepository hostRepository,
        IServiceProvider serviceProvider
    )
    {
        _hostRepository = hostRepository;
        _serviceProvider = serviceProvider;
    }

    public async Task<ErrorOr<List<PortainerHostInformation>>> Handle(
        GetPortainerHostsQuery request,
        CancellationToken cancellationToken
    )
    {
        var hostId = HostId.Create(request.HostId);
        if (await _hostRepository.FindByIdAsync(hostId, cancellationToken) is not Host host)
        {
            return Errors.Host.NotFoundWithHostId(hostId);
        }

        var createPortainerServiceResult = _serviceProvider.CreatePortainerService(host);
        if (createPortainerServiceResult.IsError)
        {
            return createPortainerServiceResult.Errors;
        }

        return await createPortainerServiceResult.Value.GetHostsAsync(cancellationToken);
    }
}
