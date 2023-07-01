using DataMedic.Application.Common.Interfaces.Infrastructure;

using MailKit.Net.Smtp;
using MailKit.Security;

using Microsoft.Extensions.Options;

using MimeKit;
using MimeKit.Text;

namespace DataMedic.Infrastructure.Email;

public sealed class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendEmailAsync(IReadOnlyCollection<string> to, string subject, string body,
        CancellationToken cancellationToken = default)
    {
        var email = new MimeMessage()
        {
            From = { new MailboxAddress(_settings.SenderDisplayName, _settings.SenderEmail) },
            Subject = subject,
            Body = new TextPart(TextFormat.Html) { Text = body }
        };
        email.To.AddRange(to.Select(MailboxAddress.Parse));

        using var smtpClient = new SmtpClient();

        await smtpClient.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort, SecureSocketOptions.None,
            cancellationToken);

        await smtpClient.SendAsync(email, cancellationToken);

        await smtpClient.DisconnectAsync(true, cancellationToken);
    }
}