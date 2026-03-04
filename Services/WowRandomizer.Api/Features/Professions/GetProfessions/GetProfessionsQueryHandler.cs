namespace WowRandomizer.Api.Features.Professions.GetProfessions;

public record GetProfessionsQuery(bool? IsPrimary) : IQuery<GetProfessionsResult>;

public record GetProfessionsResult(List<ProfessionResponse> Professions);

public record ProfessionResponse(int Id, string Name, bool IsPrimary);

public class GetProfessionsQueryHandler(AppDbContext db)
    : IQueryHandler<GetProfessionsQuery, GetProfessionsResult>
{
    public async Task<GetProfessionsResult> Handle(GetProfessionsQuery request, CancellationToken cancellationToken)
    {
        var query = db.Professions.AsQueryable();

        if (request.IsPrimary.HasValue)
            query = query.Where(p => p.IsPrimary == request.IsPrimary.Value);

        var professions = await query
            .Select(p => new ProfessionResponse(p.Id, p.Name, p.IsPrimary))
            .ToListAsync(cancellationToken);

        return new GetProfessionsResult(professions);
    }
}
