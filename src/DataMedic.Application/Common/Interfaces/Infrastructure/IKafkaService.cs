using Confluent.Kafka;

using ErrorOr;

namespace DataMedic.Application.Common.Interfaces.Infrastructure;

public interface IKafkaService
{
    ErrorOr<List<string>> ListTopics();
    public AdminClientConfig GetConfig();
}
