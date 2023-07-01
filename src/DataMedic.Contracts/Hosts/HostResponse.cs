namespace DataMedic.Contracts.Hosts;

public record HostResponse(
    Guid Id,
    string Name,
    int Type,
    IEnumerable<string> Uris,
    HostCredentialsResponse Credentials,
    HostSslConfigurationResponse SslConfiguration,
    DateTime CreatedOnUtc,
    DateTime? ModifiedOnUtc
);

public record HostCredentialsResponse(int Type, string Username, string Credential);

public record HostSslConfigurationResponse(
    SslCertificateResponse? CertificateAuthority,
    SslCertificateResponse? Certificate
);

public record SslCertificateResponse(
    string Name,
    string Authority,
    DateTime ExpiresAtUtc,
    byte[] CertificateData,
    string Passphrase
);
