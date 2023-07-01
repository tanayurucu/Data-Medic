using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Hosts;

using ErrorOr;

namespace DataMedic.Application.Hosts.Commands.CreateHost;

public sealed record CreateHostCommand(
    string Name,
    int Type,
    List<string> Uris,
    CreateHostCredentialCommand Credentials,
    CreateHostSslConfigurationCommand SslConfiguration
) : ICommand<ErrorOr<Host>>;

public sealed record CreateHostCredentialCommand(int Type, string Username, string Credential);

public sealed record CreateHostSslConfigurationCommand(
    byte[] CertificateAuthority,
    byte[] Certificate,
    string Passphrase
);
