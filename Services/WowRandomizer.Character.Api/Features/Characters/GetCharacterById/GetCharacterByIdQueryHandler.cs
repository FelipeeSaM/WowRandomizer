using Microsoft.EntityFrameworkCore;
using WowRandomizer.Character.Api.Database;
using WowRandomizer.Character.Api.Features.Characters.GetCharacters;

namespace WowRandomizer.Character.Api.Features.Characters.GetCharacterById;

public record GetCharacterByIdQuery(Guid Id) : IQuery<GetCharacterByIdResult>;

public record GetCharacterByIdResult(CharacterResponse Character);

public class GetCharacterByIdQueryHandler(CharacterDbContext db)
    : IQueryHandler<GetCharacterByIdQuery, GetCharacterByIdResult>
{
    public async Task<GetCharacterByIdResult> Handle(GetCharacterByIdQuery request, CancellationToken cancellationToken)
    {
        var character = await db.Characters
            .Where(c => c.Id == request.Id)
            .Select(c => new CharacterResponse(
                c.Id, c.Faction, c.Race, c.Class, c.Gender,
                c.Profession1, c.Profession2, c.SubProfession1, c.SubProfession2,
                c.GeneratedAt, c.SavedAt))
            .FirstOrDefaultAsync(cancellationToken);

        if (character is null)
            throw new KeyNotFoundException($"Character with id '{request.Id}' not found.");

        return new GetCharacterByIdResult(character);
    }
}
