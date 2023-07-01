namespace DataMedic.Infrastructure.Authentication;

public sealed class AuthenticationSettings
{
    public const string SectionName = nameof(AuthenticationSettings);
    public string Name { get; set; } = string.Empty;
    public string MetadataAddress { get; set; } = string.Empty;
    public string Authority { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}
