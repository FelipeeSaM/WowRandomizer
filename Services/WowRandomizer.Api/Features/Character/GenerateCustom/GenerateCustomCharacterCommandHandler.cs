using BuildingBlocks.Messaging.Events;
using MassTransit;
using WowRandomizer.Api.Features.Character;

namespace WowRandomizer.Api.Features.Character.GenerateCustom;

public record GenerateCustomCharacterCommand(
    string? FactionName,
    string? RaceName,
    string? ClassName
) : ICommand<GenerateCharacterResult>;

public class GenerateCustomCharacterCommandValidator : AbstractValidator<GenerateCustomCharacterCommand>
{
    public GenerateCustomCharacterCommandValidator()
    {
        When(x => x.FactionName is not null, () =>
            RuleFor(x => x.FactionName).NotEmpty().WithMessage("FactionName cannot be empty."));

        When(x => x.RaceName is not null, () =>
            RuleFor(x => x.RaceName).NotEmpty().WithMessage("RaceName cannot be empty."));

        When(x => x.ClassName is not null, () =>
            RuleFor(x => x.ClassName).NotEmpty().WithMessage("ClassName cannot be empty."));
    }
}

public class GenerateCustomCharacterCommandHandler(AppDbContext db, IPublishEndpoint publishEndpoint)
    : ICommandHandler<GenerateCustomCharacterCommand, GenerateCharacterResult>
{
    private static readonly string[] Genders = ["Male", "Female"];

    public async Task<GenerateCharacterResult> Handle(GenerateCustomCharacterCommand request, CancellationToken cancellationToken)
    {
        var racesQuery = db.Races
            .Include(r => r.FactionRaces).ThenInclude(fr => fr.Faction)
            .Include(r => r.RaceClasses).ThenInclude(rc => rc.Class)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.FactionName))
            racesQuery = racesQuery.Where(r => r.FactionRaces.Any(fr => fr.Faction.Name == request.FactionName));

        if (!string.IsNullOrWhiteSpace(request.ClassName))
            racesQuery = racesQuery.Where(r => r.RaceClasses.Any(rc => rc.Class.Name == request.ClassName));

        if (!string.IsNullOrWhiteSpace(request.RaceName))
            racesQuery = racesQuery.Where(r => r.Name == request.RaceName);

        var compatibleRaces = await racesQuery.ToListAsync(cancellationToken);

        if (compatibleRaces.Count == 0)
            throw new ArgumentException("No compatible race found for the given combination of faction, race and class.");

        var race = compatibleRaces[Random.Shared.Next(compatibleRaces.Count)];

        var factionName = !string.IsNullOrWhiteSpace(request.FactionName)
            ? request.FactionName
            : race.FactionRaces.Select(fr => fr.Faction.Name).ToList() is { Count: > 0 } factions
                ? factions[Random.Shared.Next(factions.Count)]
                : throw new InvalidOperationException("Race has no associated factions.");

        var className = !string.IsNullOrWhiteSpace(request.ClassName)
            ? request.ClassName
            : race.RaceClasses.Select(rc => rc.Class.Name).ToList() is { Count: > 0 } classes
                ? classes[Random.Shared.Next(classes.Count)]
                : throw new InvalidOperationException("Race has no associated classes.");

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
            factionName,
            race.Name,
            className,
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
