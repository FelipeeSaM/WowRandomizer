namespace WowRandomizer.Api.Features.Professions.GetProfessions;

public class GetProfessionsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/professions", async (ISender sender, bool? isPrimary) =>
        {
            var result = await sender.Send(new GetProfessionsQuery(isPrimary));
            return Results.Ok(result);
        })
        .WithName("GetProfessions")
        .Produces<GetProfessionsResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Professions")
        .WithDescription("Returns all professions. Filter by isPrimary=true for primary professions or isPrimary=false for secondary");
    }
}
