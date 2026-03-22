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
        var faction = await GenerateRandomFaction(cancellationToken);
        var race = await GenerateRandomRace(faction, cancellationToken);
        var gameClass = await GenerateRandomClass(race, cancellationToken);
        var gender = GenerateRandomGender();
        var pickedPrimary = await GenerateRandomProfessions(true, cancellationToken);
        var pickedSecondary = await GenerateRandomProfessions(false, cancellationToken);

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

    private async Task<Faction> GenerateRandomFaction(CancellationToken cancellationToken)
    {
        var factions = await db.Factions.ToListAsync(cancellationToken);
        return factions[Random.Shared.Next(factions.Count)];
    }

    private async Task<Race> GenerateRandomRace(Faction faction, CancellationToken cancellationToken)
    {
        var races = await db.Races
            .Where(r => r.FactionRaces.Any(fr => fr.FactionId == faction.Id))
            .ToListAsync(cancellationToken);
        return races[Random.Shared.Next(races.Count)];
    }

    private async Task<GameClass> GenerateRandomClass(Race race, CancellationToken cancellationToken)
    {
        var classes = await db.Classes
            .Where(c => c.RaceClasses.Any(rc => rc.RaceId == race.Id))
            .ToListAsync(cancellationToken);
        return classes[Random.Shared.Next(classes.Count)];
    }

    private static string GenerateRandomGender() =>
        Genders[Random.Shared.Next(Genders.Length)];

    private async Task<List<Profession>> GenerateRandomProfessions(bool isPrimary, CancellationToken cancellationToken)
    {
        var professions = await db.Professions
            .Where(p => p.IsPrimary == isPrimary)
            .ToListAsync(cancellationToken);
        return PickRandom(professions, Random.Shared.Next(3));
    }

    private static List<T> PickRandom<T>(List<T> source, int count)
    {
        if (count <= 0 || source.Count == 0) return [];
        return [.. source.OrderBy(_ => Random.Shared.Next()).Take(count)];
    }
}
