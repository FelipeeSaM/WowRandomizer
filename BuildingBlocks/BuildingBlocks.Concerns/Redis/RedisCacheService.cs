using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Concerns.Redis
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<RedisCacheService> _logger;
        private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(5);

        public RedisCacheService(IDistributedCache cache, ILogger<RedisCacheService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<T?> GetOrSetAsync<T>(
            string key,
            Func<Task<T>> factory,
            TimeSpan? expiration = null,
            CancellationToken cancellationToken = default) where T : class
        {
            var cached = await GetAsync<T>(key, cancellationToken);
            if (cached != null)
            {
                _logger.LogInformation("[Cache HIT] Key: {Key}", key);
                return cached;
            }

            _logger.LogInformation("[Cache MISS] Key: {Key} - Fetching from source", key);

            var value = await factory();

            if (value != null)
            {
                await SetAsync(key, value, expiration, cancellationToken);
            }

            return value;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
        {
            try
            {
                var cached = await _cache.GetStringAsync(key, cancellationToken);

                if (string.IsNullOrEmpty(cached))
                    return null;

                return JsonConvert.DeserializeObject<T>(cached);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Cache ERROR] Failed to get key: {Key}", key);
                return null;
            }
        }

        public async Task SetAsync<T>(
            string key,
            T value,
            TimeSpan? expiration = null,
            CancellationToken cancellationToken = default) where T : class
        {
            try
            {
                var json = JsonConvert.SerializeObject(value);

                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiration ?? DefaultExpiration
                };

                await _cache.SetStringAsync(key, json, options, cancellationToken);

                _logger.LogInformation(
                    "[Cache SET] Key: {Key}, Expiration: {Expiration}",
                    key,
                    expiration ?? DefaultExpiration);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Cache ERROR] Failed to set key: {Key}", key);
            }
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            try
            {
                await _cache.RemoveAsync(key, cancellationToken);
                _logger.LogInformation("[Cache REMOVE] Key: {Key}", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Cache ERROR] Failed to remove key: {Key}", key);
            }
        }

        public Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default)
        {
            _logger.LogWarning("[Cache] RemoveByPatternAsync not implemented for pattern: {Pattern}", pattern);
            return Task.CompletedTask;
        }
    }
}
