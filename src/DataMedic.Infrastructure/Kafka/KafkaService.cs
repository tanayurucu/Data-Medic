using System.Security.Cryptography.X509Certificates;
using Confluent.Kafka;

using DataMedic.Application.Common.Interfaces.Infrastructure;

using ErrorOr;

namespace DataMedic.Infrastructure.Kafka;

internal sealed class KafkaService : IKafkaService
{
    private readonly AdminClientConfig _config;

    public KafkaService(
        IEnumerable<string> bootstrapServers,
        X509Certificate2 ca,
        X509Certificate2 cert,
        string passphrase
    )
    {
        _config = new AdminClientConfig
        {
            BootstrapServers = string.Join(",", bootstrapServers),
            SecurityProtocol = SecurityProtocol.Ssl,
            SslCaPem = ca.ExportCertificatePem(),
            SslCertificatePem = cert.ExportCertificatePem(),
            SslKeyPem = cert.GetRSAPrivateKey()?.ExportPkcs8PrivateKeyPem(),
            SslKeyPassword = passphrase
        };
    }

    public ErrorOr<List<string>> ListTopics()
    {
        try
        {
            using var client = new AdminClientBuilder(_config).Build();

            var metadata = client.GetMetadata(TimeSpan.FromSeconds(10));

            return metadata.Topics.ConvertAll(topicMetadata => topicMetadata.Topic);
        }
        catch (Exception exception)
        {
            return ErrorOr.Error.Failure(
                "Kafka.ListTopics",
                $"Exception occurred while listing topics. Exception: {exception.Message}"
            );
        }
    }

    public AdminClientConfig GetConfig()
    {
        return _config;
    }
}
