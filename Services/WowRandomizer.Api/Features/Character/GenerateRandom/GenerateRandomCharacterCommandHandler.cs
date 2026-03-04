using BuildingBlocks.Messaging.Events;
using MassTransit;
using WowRandomizer.Api.Features.Character;

namespace WowRandomizer.Api.Features.Character.GenerateRandom;

public record GenerateRandomCharacterCommand : ICommand<GenerateCharacterResult>;

public class GenerateRandomCharacterCommandHandler(AppDbContext db, IPublishEndpoint publishEndpoint)
    : ICommandHandler<GenerateRandomCharacterCommand, GenerateCharacterResult>
{
    private static readonly string[] Genders = ["Male", "Female"];

    public async Task<GenerateCharacterResult> Handle(GenerateRandomCharacterCommand request, CancellationToken cancellationToken)
    {
        var factions = await db.Factions.ToListAsync(cancellationToken);
        var faction = factions[Random.Shared.Next(factions.Count)];

        var races = await db.Races
            .Where(r => r.FactionRaces.Any(fr => fr.FactionId == faction.Id))
            .ToListAsync(cancellationToken);
        var race = races[Random.Shared.Next(races.Count)];

        var classes = await db.Classes
            .Where(c => c.RaceClasses.Any(rc => rc.RaceId == race.Id))
            .ToListAsync(cancellationToken);
        var gameClass = classes[Random.Shared.Next(classes.Count)];

        var gender = Genders[Random.Shared.Next(Genders.Length)];

        var primaryProfessions = await db.Professions
            .Where(p => p.IsPrimary)
            .ToListAsync(cancellationToken);

        var secondaryProfessions = await db.Professions
            .Where(p => !p.IsPrimary)
            .ToListAsync(cancellationToken);

        var pickedPrimary   = PickRandom(primaryProfessions,   Random.Shared.Next(3));
        var pickedSecondary = PickRandom(secondaryProfessions, Random.Shared.Next(3));

        var result = new GenerateCharacterResult(
            Guid.NewGuid(),
            faction.Name,
            race.Name,
            gameClass.Name,
            gender,
            pickedPrimary.ElementAtOrDefault(0)?.Name,
            pickedPrimary.ElementAtOrDefault(1)?.Name,
            pickedSecondary.ElementAtOrDefault(0)?.Name,
            pickedSecondary.ElementAtOrDefault(1)?.Name,
            DateTime.UtcNow
        );

        await publishEndpoint.Publish(new CharacterGeneratedEvent(
            result.Id, result.Faction, result.Race, result.Class, result.Gender,
            result.Profession1, result.Profession2, result.SubProfession1, result.SubProfession2,
            result.GeneratedAt
        ), cancellationToken);

        return result;
    }

    private static List<T> PickRandom<T>(List<T> source, int count)
    {
        if (count <= 0 || source.Count == 0) return [];
        return [.. source.OrderBy(_ => Random.Shared.Next()).Take(count)];
    }
}
