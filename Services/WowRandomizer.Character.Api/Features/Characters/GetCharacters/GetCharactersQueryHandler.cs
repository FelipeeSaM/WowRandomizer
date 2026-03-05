using Microsoft.EntityFrameworkCore;
using WowRandomizer.Character.Api.Database;

namespace WowRandomizer.Character.Api.Features.Characters.GetCharacters;

public record GetCharactersQuery : IQuery<GetCharactersResult>;

public record GetCharactersResult(List<CharacterResponse> Characters);

public record CharacterResponse(
    Guid Id,
    string Faction,
    string Race,
    string Class,
    string Gender,
    string? Profession1,
    string? Profession2,
    string? SubProfession1,
    string? SubProfession2,
    DateTime GeneratedAt,
    DateTime SavedAt
);

public class GetCharactersQueryHandler(CharacterDbContext db)
    : IQueryHandler<GetCharactersQuery, GetCharactersResult>
{
    public async Task<GetCharactersResult> Handle(GetCharactersQuery request, CancellationToken cancellationToken)
    {
        var characters = await db.Characters
            .OrderByDescending(c => c.SavedAt)
            .Select(c => new CharacterResponse(
                c.Id, c.Faction, c.Race, c.Class, c.Gender,
                c.Profession1, c.Profession2, c.SubProfession1, c.SubProfession2,
                c.GeneratedAt, c.SavedAt))
            .ToListAsync(cancellationToken);

        return new GetCharactersResult(characters);
    }
}
