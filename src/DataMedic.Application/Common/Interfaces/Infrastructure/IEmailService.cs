namespace DataMedic.Application.Common.Interfaces.Infrastructure;

public interface IEmailService
{
    Task SendEmailAsync(
        IReadOnlyCollection<string> to,
        string subject,
        string body,
        CancellationToken cancellationToken = default
    );
}
