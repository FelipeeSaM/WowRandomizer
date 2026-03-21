

namespace WowRandomizer.Api.Features.Classes.GetClasses;

public record GetClassesQuery(string? RaceName) : IQuery<GetClassesResult>;

public record GetClassesResult(List<ClassResponse> Classes);

public record ClassResponse(int Id, string Name);

public class GetClassesQueryHandler(AppDbContext db, ICacheService cache)
    : IQueryHandler<GetClassesQuery, GetClassesResult>
{
    public async Task<GetClassesResult> Handle(GetClassesQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = string.IsNullOrWhiteSpace(request.RaceName)
            ? "classes:all"
            : $"classes:race:{request.RaceName}";

        var classes = await cache.GetOrSetAsync(
            key: cacheKey,
            factory: async () =>
            {
                var query = db.Classes.AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.RaceName))
                    query = query.Where(c => c.RaceClasses.Any(rc => rc.Race.Name == request.RaceName));

                var result = await query
                    .Select(c => new ClassResponse(c.Id, c.Name))
                    .ToListAsync(cancellationToken);

                return result;
            },
            expiration: TimeSpan.FromMinutes(30),
            cancellationToken: cancellationToken
        );

        return new GetClassesResult(classes ?? new List<ClassResponse>());
    }
}
