namespace WowRandomizer.Api.RandomizerExample.UserCase1
{
    public record UserCase1Request(int Id, string Something);

    public record UserCase1Response(string Result);

    public class UserCase1Endpoint : ICarterModule
    {
        public async void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/usercase1", async (UserCase1Request request, ISender mediator) =>
            {
                var command = request.Adapt<UserCase1Command>();
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
                .WithName("UserCase1")
                .Produces<UserCase1Response>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("User Case")
                .WithDescription("User Case1"); ;
        }
    }
}
