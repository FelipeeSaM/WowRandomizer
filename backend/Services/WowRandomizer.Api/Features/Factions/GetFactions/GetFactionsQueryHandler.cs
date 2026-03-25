namespace WowRandomizer.Api.Features.Factions.GetFactions;

public record GetFactionsQuery : IQuery<GetFactionsResult>;

public record GetFactionsResult(List<FactionResponse> Factions);

public record FactionResponse(int Id, string Name);

public class GetFactionsQueryHandler(AppDbContext db)
    : IQueryHandler<GetFactionsQuery, GetFactionsResult>
{
    public async Task<GetFactionsResult> Handle(GetFactionsQuery request, CancellationToken cancellationToken)
    {
        var factions = await db.Factions
            .Select(f => new FactionResponse(f.Id, f.Name))
            .ToListAsync(cancellationToken);

        return new GetFactionsResult(factions);
    }
}
