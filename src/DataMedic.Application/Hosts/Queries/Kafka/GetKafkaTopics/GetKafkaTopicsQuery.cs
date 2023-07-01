using DataMedic.Application.Common.Messages;

using ErrorOr;

namespace DataMedic.Application.Hosts.Queries.Kafka.GetKafkaTopics;

public sealed record GetKafkaTopicsQuery(Guid HostId) : IQuery<ErrorOr<List<string>>>;
