namespace WowRandomizer.Api.Features.Races.GetRaces;

public record GetRacesQuery(string? FactionName) : IQuery<GetRacesResult>;

public record GetRacesResult(List<RaceResponse> Races);

public record RaceResponse(int Id, string Name, bool IsNeutral, List<string> Factions);

public class GetRacesQueryHandler(AppDbContext db)
    : IQueryHandler<GetRacesQuery, GetRacesResult>
{
    public async Task<GetRacesResult> Handle(GetRacesQuery request, CancellationToken cancellationToken)
    {
        var query = db.Races.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.FactionName))
            query = query.Where(r => r.FactionRaces.Any(fr => fr.Faction.Name == request.FactionName));

        var races = await query
            .Select(r => new RaceResponse(
                r.Id,
                r.Name,
                r.IsNeutral,
                r.FactionRaces.Select(fr => fr.Faction.Name).ToList()))
            .ToListAsync(cancellationToken);

        return new GetRacesResult(races);
    }
}
