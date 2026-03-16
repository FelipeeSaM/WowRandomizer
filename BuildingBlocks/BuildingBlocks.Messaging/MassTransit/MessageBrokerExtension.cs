using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.Messaging.MassTransit
{
    public static class MessageBrokerExtension
    {
        public static IServiceCollection AddMessageBroker(
            this IServiceCollection services, 
            IConfiguration configuration, 
            Assembly? assembly = null)
        {
            services.AddMassTransit(config => 
            {
                config.SetKebabCaseEndpointNameFormatter();

                if(assembly != null)
                    config.AddConsumers(assembly);

                config.UsingRabbitMq((context, cfg) => 
                {
                    cfg.Host(new Uri(configuration["MessageBroker:Host"]!), host => 
                    {
                        host.Username(configuration["MessageBroker:UserName"]!);
                        host.Password(configuration["MessageBroker:Password"]!);
                    });

                    cfg.UseMessageRetry(retry =>
                    {
                        retry.Exponential(
                            retryLimit: 5,
                            minInterval: TimeSpan.FromSeconds(1),
                            maxInterval: TimeSpan.FromSeconds(30),
                            intervalDelta: TimeSpan.FromSeconds(2)
                        );

                        retry.Ignore<ArgumentException>();
                        retry.Ignore<ArgumentNullException>();
                        retry.Ignore<InvalidOperationException>();
                    });

                    cfg.UseCircuitBreaker(cb =>
                    {
                        cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                        cb.TripThreshold = 15;
                        cb.ActiveThreshold = 10;
                        cb.ResetInterval = TimeSpan.FromMinutes(5);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
