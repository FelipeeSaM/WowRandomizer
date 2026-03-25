namespace WowRandomizer.Character.Api.Features.Characters.GetCharacters;

public class GetCharactersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/characters", async (ISender sender) =>
        {
            var result = await sender.Send(new GetCharactersQuery());
            return Results.Ok(result);
        })
        .WithName("GetCharacters")
        .Produces<GetCharactersResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Characters")
        .WithDescription("Returns all saved characters ordered by most recent");
    }
}
