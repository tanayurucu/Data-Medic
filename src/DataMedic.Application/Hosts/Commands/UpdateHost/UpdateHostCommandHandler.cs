using System.Text;

using DataMedic.Application.Common.Interfaces.Infrastructure;
using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Hosts;
using DataMedic.Domain.Hosts.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.Hosts.Commands.UpdateHost;

public sealed class UpdateHostCommandHandler : ICommandHandler<UpdateHostCommand, ErrorOr<Updated>>
{
    private readonly IEncryptionService _encryptionService;
    private readonly IHostRepository _hostRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateHostCommandHandler(
        IHostRepository hostRepository,
        IUnitOfWork unitOfWork,
        IEncryptionService encryptionService
    )
    {
        _hostRepository = hostRepository;
        _unitOfWork = unitOfWork;
        _encryptionService = encryptionService;
    }

    public async Task<ErrorOr<Updated>> Handle(
        UpdateHostCommand request,
        CancellationToken cancellationToken
    )
    {
        var hostId = HostId.Create(request.HostId);
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
                is Host existingHostWithName
            && existingHostWithName.Id != hostId
        )
        {
            return Errors.Host.AlreadyExistsWithName(createHostNameResult.Value);
        }

        if (await _hostRepository.FindByIdAsync(hostId, cancellationToken) is not Host host)
        {
            return Errors.Host.NotFoundWithHostId(hostId);
        }

        host.SetName(createHostNameResult.Value);
        host.SetType((HostType)request.Type);
        host.SetCredential(createHostCredentialsResult.Value);
        host.SetUriList(createHostUriListResult.Value);
        host.SetSslConfiguration(createHostSslConfigurationResult.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Updated;
    }
}
