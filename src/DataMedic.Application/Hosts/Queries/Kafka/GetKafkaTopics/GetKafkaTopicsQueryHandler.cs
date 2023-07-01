using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Application.Common.Interfaces.Infrastructure;

using ErrorOr;
using IServiceProvider = DataMedic.Application.Common.Interfaces.Infrastructure.IServiceProvider;
using DataMedic.Domain.Hosts.ValueObjects;
using DataMedic.Domain.Hosts;
using DataMedic.Domain.Common.Errors;

namespace DataMedic.Application.Hosts.Queries.Kafka.GetKafkaTopics;

public sealed class GetKafkaTopicsQueryHandler
    : IQueryHandler<GetKafkaTopicsQuery, ErrorOr<List<string>>>
{
    private readonly IHostRepository _hostRepository;
    private readonly IServiceProvider _serviceProvider;

    public GetKafkaTopicsQueryHandler(
        IHostRepository hostRepository,
        IServiceProvider serviceProvider
    )
    {
        _hostRepository = hostRepository;
        _serviceProvider = serviceProvider;
    }

    public async Task<ErrorOr<List<string>>> Handle(
        GetKafkaTopicsQuery request,
        CancellationToken cancellationToken
    )
    {
        var hostId = HostId.Create(request.HostId);
        if (await _hostRepository.FindByIdAsync(hostId, cancellationToken) is not Host host)
        {
            return Errors.Host.NotFoundWithHostId(hostId);
        }

        var createKafkaServiceResult = _serviceProvider.CreateKafkaService(host);
        if (createKafkaServiceResult.IsError)
        {
            return createKafkaServiceResult.Errors;
        }

        return createKafkaServiceResult.Value.ListTopics();
    }
}
