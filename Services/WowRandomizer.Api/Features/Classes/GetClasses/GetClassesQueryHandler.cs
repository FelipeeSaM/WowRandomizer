using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace WowRandomizer.Api.Features.Classes.GetClasses;

public record GetClassesQuery(string? RaceName) : IQuery<GetClassesResult>;

public record GetClassesResult(List<ClassResponse> Classes);

public record ClassResponse(int Id, string Name);

public class GetClassesQueryHandler(AppDbContext db, IDistributedCache cache)
    : IQueryHandler<GetClassesQuery, GetClassesResult>
{
    public async Task<GetClassesResult> Handle(GetClassesQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = "GetClasses";

        var cachedData = await cache.GetStringAsync(cacheKey, cancellationToken);

        List<ClassResponse> classes;

        if (cachedData is not null)
        {
            classes = JsonSerializer.Deserialize<List<ClassResponse>>(cachedData)!;
        }
        else 
        {
            var options = new DistributedCacheEntryOptions();
            options.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

            var query = db.Classes.AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.RaceName))
                query = query.Where(c => c.RaceClasses.Any(rc => rc.Race.Name == request.RaceName));

            classes = await query
                .Select(c => new ClassResponse(c.Id, c.Name))
                .ToListAsync(cancellationToken);

            var serializedData = JsonSerializer.Serialize(classes);
            await cache.SetStringAsync(cacheKey, serializedData, options, cancellationToken);
        }

        return new GetClassesResult(classes);
    }
}