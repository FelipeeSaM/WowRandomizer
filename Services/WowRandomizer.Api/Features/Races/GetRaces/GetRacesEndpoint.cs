namespace WowRandomizer.Api.Features.Races.GetRaces;

public class GetRacesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/races", async (ISender sender, string? factionName) =>
        {
            var result = await sender.Send(new GetRacesQuery(factionName));
            return Results.Ok(result);
        })
        .WithName("GetRaces")
        .Produces<GetRacesResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Races")
        .WithDescription("Returns all races, optionally filtered by faction name");
    }
}
