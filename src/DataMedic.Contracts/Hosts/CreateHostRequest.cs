namespace DataMedic.Contracts.Hosts;

public record CreateHostRequest(
    string Name,
    int Type,
    IEnumerable<string> Uris,
    CreateHostCredentialsRequest Credentials,
    CreateHostSslConfigurationRequest SslConfiguration
);

public record CreateHostCredentialsRequest(int Type, string Username, string Credential);

public record CreateHostSslConfigurationRequest(
    byte[] CertificateAuthority,
    byte[] Certificate,
    string Passphrase
);
