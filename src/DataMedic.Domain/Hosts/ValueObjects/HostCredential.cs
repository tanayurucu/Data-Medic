using DataMedic.Domain.Common.Abstractions;
using DataMedic.Domain.Common.Errors;

using ErrorOr;

namespace DataMedic.Domain.Hosts.ValueObjects;

public sealed class HostCredential : ValueObject
{
    public static readonly HostCredential None =
        new(CredentialType.None, string.Empty, Array.Empty<byte>(), Array.Empty<byte>());
    public CredentialType Type { get; private set; }
    public string Username { get; private set; }
    public byte[] EncryptedCredential { get; private set; }
    public byte[] EncryptionIV { get; private set; }

    public bool HasCredentials =>
        Type != CredentialType.None && (Username != string.Empty || EncryptedCredential.Any());

    private HostCredential() { }

    private HostCredential(
        CredentialType credentialType,
        string username,
        byte[] encryptedCredential,
        byte[] encryptionIV
    )
    {
        Type = credentialType;
        Username = username;
        EncryptedCredential = encryptedCredential;
        EncryptionIV = encryptionIV;
    }

    public static HostCredential CreateBasicCredential(
        string username,
        byte[] encryptedPassword,
        byte[] encryptionIV
    ) => new(CredentialType.Basic, username, encryptedPassword, encryptionIV);

    public static HostCredential CreateApiKeyCredential(
        byte[] encryptedApiKey,
        byte[] encryptionIV
    ) => new(CredentialType.ApiKey, string.Empty, encryptedApiKey, encryptionIV);

    public static ErrorOr<HostCredential> CreateCredential(
        CredentialType credentialType,
        string username,
        byte[] encryptedCredential,
        byte[] encryptionIV
    ) =>
        credentialType switch
        {
            CredentialType.None => None,
            CredentialType.Basic
                => CreateBasicCredential(username, encryptedCredential, encryptionIV),
            CredentialType.ApiKey => CreateApiKeyCredential(encryptedCredential, encryptionIV),
            _ => Errors.Host.InvalidCredentialType
        };

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Type;
        yield return Username;
        yield return EncryptedCredential;
        yield return EncryptionIV;
    }
}
