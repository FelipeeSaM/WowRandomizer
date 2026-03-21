using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Concerns.Redis
{
    public static class CacheExtensions
    {
        public static IServiceCollection AddRedisCache(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var redisConnection = configuration.GetConnectionString("Redis");

            if (string.IsNullOrEmpty(redisConnection))
            {
                throw new InvalidOperationException("Redis connection string is missing");
            }

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnection;
                options.InstanceName = "WowRandomizer_";
            });

            services.AddScoped<ICacheService, RedisCacheService>();

            return services;
        }
    }
}
