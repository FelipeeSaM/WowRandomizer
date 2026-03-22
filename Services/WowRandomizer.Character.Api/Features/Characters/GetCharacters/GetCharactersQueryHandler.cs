using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
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

public class GetCharactersQueryHandler(CharacterDbContext db, IDistributedCache cache)
    : IQueryHandler<GetCharactersQuery, GetCharactersResult>
{
    public async Task<GetCharactersResult> Handle(GetCharactersQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = "CharactersList";
        var cachedData = await cache.GetStringAsync(cacheKey, cancellationToken);
        var options = new DistributedCacheEntryOptions();
        options.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
        List<CharacterResponse> characters;

        if (!string.IsNullOrEmpty(cachedData) && cachedData != "[]")
        {
            characters = JsonSerializer.Deserialize<List<CharacterResponse>>(cachedData)!;
        } 
        else
        {
            characters = await db.Characters
                .OrderByDescending(c => c.SavedAt)
                .Select(c => new CharacterResponse(
                    c.Id, c.Faction, c.Race, c.Class, c.Gender,
                    c.Profession1, c.Profession2, c.SubProfession1, c.SubProfession2,
                    c.GeneratedAt, c.SavedAt))
                .ToListAsync(cancellationToken);

            if (characters.Count > 0)
            {
                await cache.SetStringAsync(
                    cacheKey, 
                    JsonSerializer.Serialize(characters), 
                    options,
                    cancellationToken);
            }
        }

        return new GetCharactersResult(characters);
    }
}
