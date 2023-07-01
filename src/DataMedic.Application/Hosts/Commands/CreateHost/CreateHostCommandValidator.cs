using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Hosts.ValueObjects;

using FluentValidation;

namespace DataMedic.Application.Hosts.Commands.CreateHost;

public sealed class CreateHostCommandValidator : AbstractValidator<CreateHostCommand>
{
    public CreateHostCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithError(Errors.Host.HostNameRequired)
            .MaximumLength(HostName.MaxLength)
            .WithError(Errors.Host.HostNameTooLong);

        RuleFor(x => x.Type)
            .Must((value) => Enum.IsDefined(typeof(HostType), value))
            .WithError(Errors.Host.InvalidHostType);

        RuleFor(x => x.Uris).NotEmpty().WithError(Errors.Host.UriRequired);

        RuleFor(x => x.Credentials.Type)
            .Must(value => Enum.IsDefined(typeof(CredentialType), value))
            .WithError(Errors.Host.InvalidCredentialType);

        RuleFor(x => x.Credentials.Credential)
            .NotEmpty()
            .When(x => x.Credentials.Type != (int)CredentialType.None)
            .WithError(Errors.Host.CredentialRequired);

        RuleFor(x => x.SslConfiguration.Passphrase)
            .Empty()
            .When(x => !x.SslConfiguration.Certificate.Any())
            .WithError(Errors.Host.PassphraseGivenWhenNoCertificate);
    }
}
