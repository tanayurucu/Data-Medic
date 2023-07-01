using DataMedic.Application.Common.Models;
using DataMedic.Application.Common.Models.Portainer;
using DataMedic.Application.Sensors.Models;
using DataMedic.Domain.Hosts;

using ErrorOr;

namespace DataMedic.Application.Common.Interfaces.Infrastructure;

public interface IPortainerService
{
    Task<ErrorOr<List<PortainerContainerInformation>>> GetContainersAsync(
        int portainerHostId,
        CancellationToken cancellationToken = default
    );
    Task<ErrorOr<List<PortainerHostInformation>>> GetHostsAsync(
        CancellationToken cancellationToken = default
    );
    Task<ErrorOr<PortainerContainerStatus>> GetContainerInformationAsync(
        int portainerHostId,
        string containerId,
        CancellationToken cancellationToken = default
    );
    Task<ErrorOr<string>> GetContainerLogsAsync(
        int portainerHostId,
        string containerId,
        int lastLogCount = 5,
        CancellationToken cancellationToken = default
    );
    Task<string> FindContainerLogsAsync(
        int portainerHostId,
        string containerId,
        int lastLogCount = 5,
        CancellationToken cancellationToken = default
    );
    Task<ErrorOr<List<ContainerInfoFromPortainer>>> GetDockerContainerStatusListAsync(
        long portainerHostId,
        CancellationToken cancellationToken = default
    );
    Task<List<ContainerInfoFromPortainer>> FindDockerContainerStatusListAsync(
        long portainerHostId,
        CancellationToken cancellationToken = default
    );
}
