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
                var logger = context.ServiceProvider.GetRequiredService<ILoggerFactory>()
                    .CreateLogger("Polly:database");

                pipeline
                    .AddRetry(new RetryStrategyOptions
                    {
                        MaxRetryAttempts = 5,
                        Delay = TimeSpan.FromSeconds(3),
                        BackoffType = DelayBackoffType.Exponential,
                        UseJitter = true,
                        OnRetry = args =>
                        {
                            logger.LogWarning(
                                "[Polly:database] Retry {AttemptNumber} of {MaxAttempts} — Exception: {ExceptionMessage}",
                                args.AttemptNumber + 1,
                                5,
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
                                "[Polly:database] Timeout after {TimeoutSeconds}s",
                                args.Timeout.TotalSeconds);
                            return ValueTask.CompletedTask;
                        }
                    });
            });

            services.AddResiliencePipeline("cache", (pipeline, context) =>
            {
                var logger = context.ServiceProvider.GetRequiredService<ILoggerFactory>()
                    .CreateLogger("Polly:cache");

                pipeline
                    .AddRetry(new RetryStrategyOptions
                    {
                        MaxRetryAttempts = 2,
                        Delay = TimeSpan.FromSeconds(2),
                        BackoffType = DelayBackoffType.Linear,
                        OnRetry = args =>
                        {
                            logger.LogWarning(
                                "[Polly:cache] Retry {AttemptNumber} of {MaxAttempts}",
                                args.AttemptNumber + 1,
                                2);
                            return ValueTask.CompletedTask;
                        }
                    })
                    .AddTimeout(TimeSpan.FromSeconds(5));
            });

            services.AddResiliencePipeline("http-client", (pipeline, context) =>
            {
                var logger = context.ServiceProvider.GetRequiredService<ILoggerFactory>()
                    .CreateLogger("Polly:http-client");

                pipeline
                    .AddRetry(new RetryStrategyOptions
                    {
                        MaxRetryAttempts = 3,
                        Delay = TimeSpan.FromSeconds(1),
                        BackoffType = DelayBackoffType.Exponential,
                        UseJitter = true,
                        ShouldHandle = new PredicateBuilder()
                            .Handle<HttpRequestException>()
                            .Handle<TaskCanceledException>(),
                        OnRetry = args =>
                        {
                            logger.LogWarning(
                                "[Polly:http-client] Retry {AttemptNumber} of {MaxAttempts} — Exception: {ExceptionMessage}",
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
                                "[Polly:http-client] Timeout after {TimeoutSeconds}s",
                                args.Timeout.TotalSeconds);
                            return ValueTask.CompletedTask;
                        }
                    });
            });

            services.AddResiliencePipeline("message-broker", (pipeline, context) =>
            {
                var logger = context.ServiceProvider.GetRequiredService<ILoggerFactory>()
                    .CreateLogger("Polly:message-broker");

                pipeline
                    .AddRetry(new RetryStrategyOptions
                    {
                        MaxRetryAttempts = 5,
                        Delay = TimeSpan.FromSeconds(2),
                        BackoffType = DelayBackoffType.Exponential,
                        UseJitter = true,
                        OnRetry = args =>
                        {
                            logger.LogWarning(
                                "[Polly:message-broker] Retry {AttemptNumber} of {MaxAttempts} — Exception: {ExceptionMessage}",
                                args.AttemptNumber + 1,
                                5,
                                args.Outcome.Exception?.Message);
                            return ValueTask.CompletedTask;
                        }
                    })
                    .AddTimeout(TimeSpan.FromSeconds(10));
            });

            return services;
        }
    }
}
