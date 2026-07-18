using HealthAxis.Api.Services.Interface;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace HealthAxis.Api.Services.Impl
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache distributedCache;

        private readonly ILogger<CacheService> logger;
        private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
        public CacheService(
            IDistributedCache distributedCache,
            ILogger<CacheService> logger)
        {
            this.distributedCache = distributedCache;
            this.logger = logger;
        }

        public async Task<T?> GetAsync<T>(
            string key,
            CancellationToken cancellationToken = default)
        {
            var cachedValue = await distributedCache.GetStringAsync(
                key,
                cancellationToken);

            if (string.IsNullOrWhiteSpace(cachedValue))
            {
                return default;
            }

            try
            {
                return JsonSerializer.Deserialize<T>(
    cachedValue,
    JsonSerializerOptions);
            }
            catch (JsonException ex)
            {
                logger.LogWarning(
                    ex,
                    "Failed to deserialize cached value for key {CacheKey}. Removing invalid cache entry.",
                    key);

                await RemoveAsync(key, cancellationToken);

                return default;
            }
        }

        public async Task SetAsync<T>(
            string key,
            T value,
            TimeSpan absoluteExpirationRelativeToNow,
            CancellationToken cancellationToken = default)
        {
            var serializedValue = JsonSerializer.Serialize(value);

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow
            };

            await distributedCache.SetStringAsync(
                key,
                serializedValue,
                cacheOptions,
                cancellationToken);
        }

        public async Task RemoveAsync(
            string key,
            CancellationToken cancellationToken = default)
        {
            await distributedCache.RemoveAsync(
                key,
                cancellationToken);
        }
    }
}