namespace WowRandomizer.Api.Features.Classes.GetClasses;

public record GetClassesQuery(string? RaceName) : IQuery<GetClassesResult>;

public record GetClassesResult(List<ClassResponse> Classes);

public record ClassResponse(int Id, string Name);

public class GetClassesQueryHandler(AppDbContext db)
    : IQueryHandler<GetClassesQuery, GetClassesResult>
{
    public async Task<GetClassesResult> Handle(GetClassesQuery request, CancellationToken cancellationToken)
    {
        var query = db.Classes.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.RaceName))
            query = query.Where(c => c.RaceClasses.Any(rc => rc.Race.Name == request.RaceName));

        var classes = await query
            .Select(c => new ClassResponse(c.Id, c.Name))
            .ToListAsync(cancellationToken);

        return new GetClassesResult(classes);
    }
}
