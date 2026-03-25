using WowRandomizer.Character.Api.Features.Characters.GetCharacterById;

namespace WowRandomizer.Character.Api.Features.Characters.GetCharacterById;

public class GetCharacterByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/characters/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetCharacterByIdQuery(id));
            return Results.Ok(result);
        })
        .WithName("GetCharacterById")
        .Produces<GetCharacterByIdResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Character By Id")
        .WithDescription("Returns a single saved character by its Id");
    }
}
