namespace DataMedic.Contracts.Hosts;

public record UpdateHostRequest(
    string Name,
    int Type,
    IEnumerable<string> Uris,
    UpdateHostCredentialsRequest Credentials,
    UpdateHostSslConfigurationRequest SslConfiguration
);

public record UpdateHostCredentialsRequest(int Type, string Username, string Credential);

public record UpdateHostSslConfigurationRequest(
    byte[] CertificateAuthority,
    byte[] Certificate,
    string Passphrase
);
