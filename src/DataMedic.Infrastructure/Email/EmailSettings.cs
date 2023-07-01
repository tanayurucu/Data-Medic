namespace DataMedic.Infrastructure.Email;

public sealed class EmailSettings
{
    public const string SectionName = nameof(EmailSettings);
    public string SenderDisplayName { get; set; } = string.Empty;
    public string SenderEmail { get; set; } = string.Empty;
    public string FrontendBaseAddress { get; set; } = string.Empty;
    public string SmtpServer { get; set; } = string.Empty;
    public int SmtpPort { get; set; }
}