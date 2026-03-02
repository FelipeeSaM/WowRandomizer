namespace WowRandomizer.Api.RandomizerExample.UserCase2
{
    public record UserCase2Request();

    public record UserCase2Response(List<string> objectList);

    public class UserCase2Endpoint : ICarterModule
    {
        public async void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/usercase2", async (ISender sender) => {
                var result = sender.Send(new UserCase2Query());

                return Results.Ok(result);
            });
        }
    }
}
