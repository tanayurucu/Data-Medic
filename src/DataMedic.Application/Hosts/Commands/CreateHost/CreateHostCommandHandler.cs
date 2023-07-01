using System.Text;
using DataMedic.Application.Common.Interfaces.Infrastructure;
using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Hosts;
using DataMedic.Domain.Hosts.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.Hosts.Commands.CreateHost;

public sealed class CreateHostCommandHandler : ICommandHandler<CreateHostCommand, ErrorOr<Host>>
{
    private readonly IEncryptionService _encryptionService;
    private readonly IHostRepository _hostRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateHostCommandHandler(
        IHostRepository hostRepository,
        IUnitOfWork unitOfWork,
        IEncryptionService encryptionService
    )
    {
        _hostRepository = hostRepository;
        _unitOfWork = unitOfWork;
        _encryptionService = encryptionService;
    }

    public async Task<ErrorOr<Host>> Handle(
        CreateHostCommand request,
        CancellationToken cancellationToken
    )
    {
        var createHostNameResult = HostName.Create(request.Name);
        var createHostUriListResult = HostUri.CreateMany(request.Uris);

        byte[] encryptedCredential = Array.Empty<byte>();
        byte[] encryptionIV = Array.Empty<byte>();
        if (request.Credentials.Type != (int)CredentialType.None)
        {
            if (string.IsNullOrWhiteSpace(request.Credentials.Credential))
            {
                return Errors.Host.CredentialRequired;
            }

            encryptedCredential = _encryptionService.Encrypt(
                Encoding.UTF8.GetBytes(request.Credentials.Credential),
                out encryptionIV
            );
        }

        var createHostCredentialsResult = HostCredential.CreateCredential(
            (CredentialType)request.Credentials.Type,
            request.Credentials.Username,
            encryptedCredential,
            encryptionIV
        );

        var createHostSslConfigurationResult = HostSslConfiguration.Create(
            request.SslConfiguration.CertificateAuthority,
            request.SslConfiguration.Certificate,
            request.SslConfiguration.Passphrase
        );

        if (
            Errors.Combine(
                createHostNameResult,
                createHostUriListResult,
                createHostCredentialsResult,
                createHostSslConfigurationResult
            )
                is List<Error> errors
            && errors.Any()
        )
        {
            return errors;
        }

        if (!Enum.IsDefined(typeof(HostType), request.Type))
        {
            return Errors.Host.InvalidHostType;
        }

        if (
            await _hostRepository.FindByNameAsync(createHostNameResult.Value, cancellationToken)
            is not null
        )
        {
            return Errors.Host.AlreadyExistsWithName(createHostNameResult.Value);
        }

        var host = Host.Create(
            createHostNameResult.Value,
            (HostType)request.Type,
            createHostUriListResult.Value,
            createHostCredentialsResult.Value,
            createHostSslConfigurationResult.Value
        );

        await _hostRepository.AddAsync(host, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return host;
    }
}
