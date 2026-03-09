using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using Polly.Timeout;

namespace BuildingBlocks.Concerns.Polly
{
    public static class PollyConfiguration
    {
        public static IServiceCollection PollyConfigure(this IServiceCollection services)
        {
            services.AddResiliencePipeline("database", (pipeline, context) =>
            {
                var logger = context.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Polly:database");

                pipeline
                    .AddRetry(new RetryStrategyOptions
                    {
                        MaxRetryAttempts = 3,
                        Delay            = TimeSpan.FromSeconds(2),
                        BackoffType      = DelayBackoffType.Exponential,
                        UseJitter        = true,
                        OnRetry = args =>
                        {
                            logger.LogWarning(
                                "[Polly:database] Retry {AttemptNumber} of {MaxAttempts} — Exception: {ExceptionMessage}",
                                args.AttemptNumber + 1,
                                3,
                                args.Outcome.Exception?.Message);
                            return ValueTask.CompletedTask;
                        }
                    })
                    .AddTimeout(new TimeoutStrategyOptions
                    {
                        Timeout = TimeSpan.FromSeconds(30),
                        OnTimeout = args =>
                        {
                            logger.LogError(
                                "[Polly:database] Timeout after {TimeoutSeconds}s — operation cancelled",
                                args.Timeout.TotalSeconds);
                            return ValueTask.CompletedTask;
                        }
                    });
            });

            services.AddResiliencePipeline("cache", (pipeline, context) =>
            {
                var logger = context.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Polly:cache");

                pipeline
                    .AddRetry(new RetryStrategyOptions
                    {
                        MaxRetryAttempts = 2,
                        Delay            = TimeSpan.FromSeconds(2),
                        BackoffType      = DelayBackoffType.Linear,
                        OnRetry = args =>
                        {
                            logger.LogWarning(
                                "[Polly:cache] Retry {AttemptNumber} of {MaxAttempts} — Exception: {ExceptionMessage}",
                                args.AttemptNumber + 1,
                                2,
                                args.Outcome.Exception?.Message);
                            return ValueTask.CompletedTask;
                        }
                    })
                    .AddTimeout(new TimeoutStrategyOptions
                    {
                        Timeout = TimeSpan.FromSeconds(5),
                        OnTimeout = args =>
                        {
                            logger.LogError(
                                "[Polly:cache] Timeout after {TimeoutSeconds}s — operation cancelled",
                                args.Timeout.TotalSeconds);
                            return ValueTask.CompletedTask;
                        }
                    });
            });

            return services;
        }
    }
}
