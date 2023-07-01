using DataMedic.Application.Common.Messages;

using ErrorOr;

namespace DataMedic.Application.Hosts.Commands.UpdateHost;

public sealed record UpdateHostCommand(
    Guid HostId,
    string Name,
    int Type,
    List<string> Uris,
    UpdateHostCredentialsCommand Credentials,
    UpdateHostSslConfigurationCommand SslConfiguration
) : ICommand<ErrorOr<Updated>>;

public sealed record UpdateHostCredentialsCommand(int Type, string Username, string Credential);

public sealed record UpdateHostSslConfigurationCommand(
    byte[] CertificateAuthority,
    byte[] Certificate,
    string Passphrase
);
