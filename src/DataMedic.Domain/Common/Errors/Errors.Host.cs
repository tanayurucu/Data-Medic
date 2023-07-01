using System.Text.Json;

using DataMedic.Domain.Hosts.ValueObjects;

using ErrorOr;

namespace DataMedic.Domain.Common.Errors;

public static partial class Errors
{
    public static class Host
    {
        public static Error HostIdRequired =>
            Error.Validation(code: "Host.HostId.Required", description: "Host ID is required.");

        public static Error HostNameRequired =>
            Error.Validation(code: "Host.HostName.Required", description: "Host name is required.");

        public static Error HostNameTooLong =>
            Error.Validation(
                code: "Host.HostName.TooLong",
                description: $"Host name must be at most {HostName.MaxLength} characters long."
            );

        public static Func<HostId, Error> NotFoundWithHostId =>
            id =>
                Error.NotFound(
                    code: "Host.NotFound.WithHostId",
                    description: $"Host with ID '{id.Value}' not found"
                );

        public static Func<HostName, Error> AlreadyExistsWithName =>
            name =>
                Error.Validation(
                    code: "Host.AlreadyExists.WithName",
                    description: $"Host with name '{name.Value}' already exists."
                );

        public static Error CredentialRequired =>
            Error.Conflict(
                code: "Host.Credential.Required",
                description: "Credential is required when credential type is not selected as none."
            );
        public static Error InvalidCredentialType =>
            Error.Validation(
                code: "Host.CredentialType.Invalid",
                description: $"Credential type is invalid. Valid values are {JsonSerializer.Serialize(Enum.GetValues(typeof(CredentialType)))}"
            );

        public static Error CertificateAuthorityInvalidAccess =>
            Error.Conflict(
                code: "Host.CertificateAuthority.InvalidAccess",
                description: "There is no certificate authority for this SSL configuration."
            );
        public static Error CertificateInvalidAccess =>
            Error.Conflict(
                code: "Host.Certificate.InvalidAccess",
                description: "There is no certificate for this SSL configuration."
            );
        public static Error PassphraseGivenWhenNoCertificate =>
            Error.Validation(
                code: "Host.Passphrase.GivenWhenNoCertificate",
                description: "Passphrase is given when there is no SSL certificate."
            );
        public static Func<string, Error> InvalidSslCertificate =>
            error =>
                Error.Validation(
                    code: "Host.SslCertificate.Invalid",
                    description: $"Given SSL certificate is invalid. Error: {error}"
                );

        public static Error InvalidHostType =>
            Error.Validation(
                code: "Host.HostType.Invalid",
                description: "Given host type is invalid."
            );

        public static Error DuplicateUri =>
            Error.Conflict(
                code: "Host.Uri.Duplicate",
                description: "Host URI list contains duplicate."
            );

        public static Func<string, Error> InvalidUri =>
            exception =>
                Error.Validation(
                    code: "Host.Uri.Invalid",
                    description: $"Invalid URI given. Error: {exception}"
                );

        public static Error HostAndSensorTypeNotMatch =>
            Error.Conflict(
                code: "Host.Type.NotMatchWithSensor",
                description: "Given host's type does not match with given sensor type."
            );

        public static Error UriRequired =>
            Error.Validation(
                code: "Host.Uri.Required",
                description: "At least one host uri is required."
            );
    }
}
