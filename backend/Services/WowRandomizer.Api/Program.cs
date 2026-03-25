using BuildingBlocks.Concerns.Logging;
using BuildingBlocks.Concerns.Polly;
using BuildingBlocks.Concerns.Redis;
using BuildingBlocks.Messaging.MassTransit;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WowRandomizer.Api.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriloggerConfiguration.Configure);

builder.Services.PollyConfigure();

builder.Services.AddCarter(new DependencyContextAssemblyCatalog([typeof(Program).Assembly]));

builder.Services.AddMediatR(config => {
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

var redisConfig = builder.Configuration.GetSection("Redis").Get<RedisConfig>();

builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.InstanceName = redisConfig!.InstanceName;
    opt.Configuration = redisConfig.Configuration;
});

builder.Services.AddMessageBroker(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();
    await DataSeeder.SeedAsync(context);
}

app.MapCarter();

app.Run();
