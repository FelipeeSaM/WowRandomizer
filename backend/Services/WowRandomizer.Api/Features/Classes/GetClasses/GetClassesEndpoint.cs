namespace WowRandomizer.Api.Features.Classes.GetClasses;

public class GetClassesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/classes", async (ISender sender, string? raceName) =>
        {
            var result = await sender.Send(new GetClassesQuery(raceName));
            return Results.Ok(result);
        })
        .WithName("GetClasses")
        .Produces<GetClassesResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Classes")
        .WithDescription("Returns all classes, optionally filtered by race name");
    }
}
