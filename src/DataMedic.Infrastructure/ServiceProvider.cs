using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using DataMedic.Application.Common.Interfaces.Infrastructure;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Hosts;
using DataMedic.Domain.Hosts.ValueObjects;
using DataMedic.Infrastructure.Email;
using DataMedic.Infrastructure.Kafka;
using DataMedic.Infrastructure.Services;

using ErrorOr;

using Microsoft.Extensions.Options;

using IServiceProvider = DataMedic.Application.Common.Interfaces.Infrastructure.IServiceProvider;

namespace DataMedic.Infrastructure;

internal sealed class ServiceProvider : IServiceProvider
{
    private readonly IEncryptionService _encryptionService;
    private readonly IOptions<EmailSettings> _emailSettings;

    public ServiceProvider(IEncryptionService encryptionService, IOptions<EmailSettings> emailSettings)
    {
        _encryptionService = encryptionService;
        _emailSettings = emailSettings;
    }

    public ErrorOr<IKafkaService> CreateKafkaService(Host host)
    {
        if (host.Type != HostType.KAFKA)
        {
            return Errors.Host.InvalidHostType;
        }
        // TODO : validate
        return new KafkaService(
            host.Uris.Select(uri => uri.Value),
            host.SslConfiguration.GetCertificateAuthority().Value,
            host.SslConfiguration.GetCertificate().Value,
            host.SslConfiguration.Passphrase
        );
    }

    public ErrorOr<INodeRedService> CreateNodeRedService(Host host)
    {
        throw new NotImplementedException();
    }

    public ErrorOr<IEmailService> CreateEmailService()
    {
        return new EmailService(_emailSettings);
    }

    public ErrorOr<IPingService> CreatePingService(Host host)
    {
        throw new NotImplementedException();
    }

    public ErrorOr<IPortainerService> CreatePortainerService(Host host)
    {
        if (host.Type != HostType.DOCKER)
        {
            return Errors.Host.InvalidHostType;
        }

        // TODO: validate api key exists
        var baseAddress = host.Uris[0].Uri;
        var apiKey = _encryptionService.Decrypt(
            host.Credential.EncryptedCredential,
            host.Credential.EncryptionIV
        );
        return new PortainerService(baseAddress, Encoding.UTF8.GetString(apiKey));
    }
}
