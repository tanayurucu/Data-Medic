using System.Security.Cryptography.X509Certificates;
using DataMedic.Domain.Common.Abstractions;
using DataMedic.Domain.Common.Errors;

using ErrorOr;

namespace DataMedic.Domain.Hosts.ValueObjects;

public sealed class HostSslConfiguration : ValueObject
{
    public byte[] CertificateAuthorityData { get; private set; }
    public byte[] CertificateData { get; private set; }
    public string Passphrase { get; private set; }
    public bool HasCertificateAuthority => CertificateAuthorityData.Any();
    public bool HasCertificate => CertificateData.Any();
    public bool HasSslConfiguration => HasCertificateAuthority || HasCertificate;

    private HostSslConfiguration(
        byte[] certificateAuthorityData,
        byte[] certificateData,
        string passphrase
    )
    {
        CertificateAuthorityData = certificateAuthorityData;
        CertificateData = certificateData;
        Passphrase = passphrase;
    }

    private HostSslConfiguration() { }

    public ErrorOr<X509Certificate2> GetCertificateAuthority()
    {
        if (!HasCertificateAuthority)
        {
            return Errors.Host.CertificateAuthorityInvalidAccess;
        }

        return new X509Certificate2(CertificateAuthorityData);
    }

    public ErrorOr<X509Certificate2> GetCertificate()
    {
        if (!HasCertificate)
        {
            return Errors.Host.CertificateInvalidAccess;
        }
        var cert = new X509Certificate2(
            CertificateData,
            Passphrase,
            X509KeyStorageFlags.Exportable
        );
        return cert;
    }

    private static ErrorOr<Success> ValidateCertificateData(
        byte[] certificateData,
        string? passphrase = null
    )
    {
        try
        {
            if (certificateData.Any())
            {
                var _ = new X509Certificate2(certificateData, passphrase);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(passphrase))
                {
                    return Errors.Host.PassphraseGivenWhenNoCertificate;
                }
            }

            return Result.Success;
        }
        catch (Exception exception)
        {
            return Errors.Host.InvalidSslCertificate(exception.Message);
        }
    }

    public ErrorOr<Updated> SetCertificateAuthority(byte[] certificateAuthorityData)
    {
        var certificateValidationResult = ValidateCertificateData(certificateAuthorityData);
        if (certificateValidationResult.IsError)
        {
            return certificateValidationResult.Errors;
        }

        CertificateAuthorityData = certificateAuthorityData;

        return Result.Updated;
    }

    public ErrorOr<Updated> SetCertificate(byte[] certificateData, string passphrase)
    {
        var certificateValidationResult = ValidateCertificateData(certificateData, passphrase);
        if (certificateValidationResult.IsError)
        {
            return certificateValidationResult.Errors;
        }

        CertificateData = certificateData;
        Passphrase = passphrase;

        return Result.Updated;
    }

    public static ErrorOr<HostSslConfiguration> Create(
        byte[] certificateAuthorityData,
        byte[] certificateData,
        string passphrase
    )
    {
        if (
            Errors.Combine(
                ValidateCertificateData(certificateAuthorityData),
                ValidateCertificateData(certificateData, passphrase)
            )
                is List<Error> errors
            && errors.Any()
        )
        {
            return errors;
        }
        return new HostSslConfiguration(certificateAuthorityData, certificateData, passphrase);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CertificateAuthorityData;
        yield return CertificateData;
        yield return Passphrase;
    }
}
