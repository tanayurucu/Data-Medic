using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Hosts;
using DataMedic.Domain.Hosts.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.Hosts.Commands.DeleteHost;

public sealed class DeleteHostCommandHandler : ICommandHandler<DeleteHostCommand, ErrorOr<Deleted>>
{
    private readonly IHostRepository _hostRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteHostCommandHandler(IHostRepository hostRepository, IUnitOfWork unitOfWork)
    {
        _hostRepository = hostRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Deleted>> Handle(
        DeleteHostCommand request,
        CancellationToken cancellationToken
    )
    {
        var hostId = HostId.Create(request.HostId);
        if (await _hostRepository.FindByIdAsync(hostId, cancellationToken) is not Host host)
        {
            return Errors.Host.NotFoundWithHostId(hostId);
        }

        _hostRepository.Remove(host);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }
}
