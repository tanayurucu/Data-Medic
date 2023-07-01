using DataMedic.Domain.Hosts;

using ErrorOr;

namespace DataMedic.Application.Common.Interfaces.Infrastructure;

public interface IServiceProvider
{
    ErrorOr<IPingService> CreatePingService(Host host);

    ErrorOr<IPortainerService> CreatePortainerService(Host host);

    ErrorOr<IKafkaService> CreateKafkaService(Host host);

    ErrorOr<INodeRedService> CreateNodeRedService(Host host);

    ErrorOr<IEmailService> CreateEmailService();
}
