using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Concerns.Redis
{
    public interface ICacheService
    {
        /// <summary>
        /// Get a value from the cache or execute the function to get and store it
        /// </summary>
        Task<T?> GetOrSetAsync<T>(
            string key,
            Func<Task<T>> factory,
            TimeSpan? expiration = null,
            CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        /// Get th cache value
        /// </summary>
        Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        /// Store cache value
        /// </summary>
        Task SetAsync<T>(
            string key,
            T value,
            TimeSpan? expiration = null,
            CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        /// Remove cache value
        /// </summary>
        Task RemoveAsync(string key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Remove cache values by pattern
        /// </summary>
        Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default);
    }
}
