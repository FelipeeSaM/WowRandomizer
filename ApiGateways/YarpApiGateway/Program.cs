using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpClientDefaults(http =>
{
    http.AddStandardResilienceHandler(options =>
    {
        options.Retry.MaxRetryAttempts = 3;
        options.Retry.Delay = TimeSpan.FromSeconds(1);
        options.Retry.BackoffType = Polly.DelayBackoffType.Exponential;
        options.Retry.UseJitter = true;
        options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(30);
        options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(10);
        options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(30);
        options.CircuitBreaker.FailureRatio = 0.5;
        options.CircuitBreaker.MinimumThroughput = 5;
    });
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddRateLimiter(rateLimiterOptions => 
{
    rateLimiterOptions.AddFixedWindowLimiter("fixed", options => 
    {
        options.Window = TimeSpan.FromSeconds(10);
        options.PermitLimit = 100;
        options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 10;
    });
    
    rateLimiterOptions.AddFixedWindowLimiter("generator", options =>
    {
        options.Window = TimeSpan.FromSeconds(60);
        options.PermitLimit = 10;
    });
});

var app = builder.Build();

app.UseRateLimiter();

app.MapReverseProxy(proxyPipeline =>
{
    proxyPipeline.Use(async (context, next) =>
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        var startTime = DateTime.UtcNow;
        
        logger.LogInformation(
            "[YARP Gateway] {Method} {Path} → Forwarding to backend service",
            context.Request.Method,
            context.Request.Path);
        
        await next();
        
        var duration = DateTime.UtcNow - startTime;
        
        logger.LogInformation(
            "[YARP Gateway] {Method} {Path} ← {StatusCode} (took {Duration}ms)",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            duration.TotalMilliseconds);
    });
});

app.Run();