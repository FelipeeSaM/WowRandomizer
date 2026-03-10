using BuildingBlocks.Concerns.Logging;
using BuildingBlocks.Concerns.Polly;
using BuildingBlocks.Messaging.MassTransit;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WowRandomizer.Character.Api.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriloggerConfiguration.Configure);

builder.Services.PollyConfigure();

builder.Services.AddCarter(new DependencyContextAssemblyCatalog([typeof(Program).Assembly]));

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddDbContext<CharacterDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddMessageBroker(builder.Configuration, typeof(Program).Assembly);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CharacterDbContext>();
    await context.Database.MigrateAsync();
}

app.MapCarter();

app.Run();
