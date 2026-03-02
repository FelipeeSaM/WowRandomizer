namespace WowRandomizer.Api.RandomizerExample.UserCase2
{
    public record UserCase2Query() : IQuery<UserCase2Result>;

    public record UserCase2Result(List<string> results);
    public class UserCase2QueryHandler
        : IQueryHandler<UserCase2Query, UserCase2Result>
    {
        public async Task<UserCase2Result> Handle(UserCase2Query request, CancellationToken cancellationToken)
        {
            // logics
            return new UserCase2Result(new List<string>() { "result1", "result2" });
        }
    }
}
