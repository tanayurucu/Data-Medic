namespace DataMedic.Infrastructure.Cryptography;

public sealed class EncryptionSettings
{
    public const string SectionName = nameof(EncryptionSettings);
    public string Key { get; set; } = string.Empty;
}
