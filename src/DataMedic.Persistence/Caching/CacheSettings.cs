namespace DataMedic.Persistence.Caching;

public sealed class CacheSettings
{
    public const string SectionName = nameof(CacheSettings);
    public string Prefix { get; set; } = string.Empty;
    public int TimeToLiveInMilliseconds { get; set; }
}