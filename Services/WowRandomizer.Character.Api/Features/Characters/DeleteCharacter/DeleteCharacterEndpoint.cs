using WowRandomizer.Character.Api.Features.Characters.DeleteCharacter;

namespace WowRandomizer.Character.Api.Features.Characters.DeleteCharacter;

public class DeleteCharacterEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/characters/{id:guid}", async (Guid id, ISender sender) =>
        {
            await sender.Send(new DeleteCharacterCommand(id));
            return Results.NoContent();
        })
        .WithName("DeleteCharacter")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Delete Character")
        .WithDescription("Deletes a saved character by its Id");
    }
}
