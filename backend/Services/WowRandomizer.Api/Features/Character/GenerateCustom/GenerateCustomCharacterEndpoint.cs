using WowRandomizer.Api.Features.Character;
using WowRandomizer.Api.Features.Character.GenerateCustom;

namespace WowRandomizer.Api.Features.Character.GenerateCustom;

public record GenerateCustomCharacterRequest(
    string? FactionName,
    string? RaceName,
    string? ClassName,
    List<Profession>? MainProfession,
    List<Profession>? SubProfession
);

public class GenerateCustomCharacterEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/character/generate/custom", async (GenerateCustomCharacterRequest request, ISender sender) =>
        {
            var command = request.Adapt<GenerateCustomCharacterCommand>();
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName("GenerateCustomCharacter")
        .Produces<GenerateCharacterResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Generate Custom Character")
        .WithDescription("Generates a WoW character based on optional fixed parameters: FactionName, RaceName and/or ClassName");
    }
}
