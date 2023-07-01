using DataMedic.Application.Common.Interfaces.Persistence;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

namespace DataMedic.Persistence.Caching;

internal sealed class CacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;
    private readonly CacheSettings _settings;

    public CacheService(IDistributedCache distributedCache, IOptions<CacheSettings> settings)
    {
        _distributedCache = distributedCache;
        _settings = settings.Value;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        var cachedValue =
            await _distributedCache.GetStringAsync(string.Concat(_settings.Prefix, key), cancellationToken);

        return cachedValue is null
            ? null
            : JsonConvert.DeserializeObject<T>(cachedValue,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
    }

    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
    {
        var cacheValue = JsonConvert.SerializeObject(value,
            new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

        await _distributedCache.SetStringAsync(string.Concat(_settings.Prefix, key), cacheValue,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(_settings.TimeToLiveInMilliseconds)
            }, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(string.Concat(_settings.Prefix, key), cancellationToken);
    }
}