namespace WowRandomizer.Api.Features.Factions.GetFactions;

public class GetFactionsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/factions", async (ISender sender) =>
        {
            var result = await sender.Send(new GetFactionsQuery());
            return Results.Ok(result);
        })
        .WithName("GetFactions")
        .Produces<GetFactionsResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Factions")
        .WithDescription("Returns all available factions");
    }
}
