using WowRandomizer.Api.Features.Character;
using WowRandomizer.Api.Features.Character.GenerateRandom;

namespace WowRandomizer.Api.Features.Character.GenerateRandom;

public class GenerateRandomCharacterEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/character/generate/random", async (ISender sender) =>
        {
            var result = await sender.Send(new GenerateRandomCharacterCommand());
            return Results.Ok(result);
        })
        .WithName("GenerateRandomCharacter")
        .Produces<GenerateCharacterResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Generate Random Character")
        .WithDescription("Generates a fully random WoW character with faction, race, class, gender and professions");
    }
}
