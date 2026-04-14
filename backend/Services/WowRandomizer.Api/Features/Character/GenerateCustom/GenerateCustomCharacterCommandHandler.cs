using BuildingBlocks.Messaging.Events;
using MassTransit;
using Polly.Registry;
using WowRandomizer.Api.Features.Character;

namespace WowRandomizer.Api.Features.Character.GenerateCustom;

public record GenerateCustomCharacterCommand(
    string? FactionName,
    string? RaceName,
    string? ClassName,
    List<Profession>? MainProfession,
    List<Profession>? SubProfession
) : ICommand<GenerateCharacterResult>;

public class GenerateCustomCharacterCommandValidator : AbstractValidator<GenerateCustomCharacterCommand>
{
    public GenerateCustomCharacterCommandValidator()
    {
        RuleFor(x => x.FactionName).NotEmpty().WithMessage("FactionName cannot be empty.");
    }
}

public class GenerateCustomCharacterCommandHandler(AppDbContext db, IPublishEndpoint publishEndpoint)
    : ICommandHandler<GenerateCustomCharacterCommand, GenerateCharacterResult>
{
    private static readonly string[] Genders = ["Male", "Female"];

    public async Task<GenerateCharacterResult> Handle(GenerateCustomCharacterCommand request, CancellationToken cancellationToken)
    {
        var race = await ResolveRace(request, cancellationToken);
        var factionName = ResolveFactionName(request, race);
        var className = ResolveClassName(request, race);
        var gender = GenerateRandomGender();

        var pickedPrimary   = await GenerateRandomProfessions(isPrimary: true, request.MainProfession, cancellationToken);
        var pickedSecondary = await GenerateRandomProfessions(isPrimary: false, request.SubProfession, cancellationToken);

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

    private async Task<Race> ResolveRace(GenerateCustomCharacterCommand request, CancellationToken cancellationToken)
    {
        var query = db.Races
            .Include(r => r.FactionRaces).ThenInclude(fr => fr.Faction)
            .Include(r => r.RaceClasses).ThenInclude(rc => rc.Class)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.FactionName))
            query = query.Where(r => r.FactionRaces.Any(fr => fr.Faction.Name == request.FactionName));

        if (!string.IsNullOrWhiteSpace(request.ClassName))
            query = query.Where(r => r.RaceClasses.Any(rc => rc.Class.Name == request.ClassName));

        if (!string.IsNullOrWhiteSpace(request.RaceName))
            query = query.Where(r => r.Name == request.RaceName);

        var compatibleRaces = await query.ToListAsync(cancellationToken);

        if (compatibleRaces.Count == 0)
            throw new ArgumentException("No compatible race found for the given combination of faction, race and class.");

        return compatibleRaces[Random.Shared.Next(compatibleRaces.Count)];
    }

    private static string ResolveFactionName(GenerateCustomCharacterCommand request, Race race) =>
        !string.IsNullOrWhiteSpace(request.FactionName)
            ? request.FactionName
            : race.FactionRaces.Select(fr => fr.Faction.Name).ToList() is { Count: > 0 } factions
                ? factions[Random.Shared.Next(factions.Count)]
                : throw new InvalidOperationException("Race has no associated factions.");

    private static string ResolveClassName(GenerateCustomCharacterCommand request, Race race) =>
        !string.IsNullOrWhiteSpace(request.ClassName)
            ? request.ClassName
            : race.RaceClasses.Select(rc => rc.Class.Name).ToList() is { Count: > 0 } classes
                ? classes[Random.Shared.Next(classes.Count)]
                : throw new InvalidOperationException("Race has no associated classes.");

    private static string GenerateRandomGender() =>
        Genders[Random.Shared.Next(Genders.Length)];

    private async Task<List<Profession>> GenerateRandomProfessions(bool isPrimary, List<Profession>? professionsList, CancellationToken cancellationToken)
    {
        professionsList ??= [];
        if (professionsList.Count == 2) return professionsList.Where(c => c.IsPrimary == isPrimary).ToList();
        List<Profession> professionsReturn = new List<Profession>();
        var professions = await db.Professions
            .Where(p => p.IsPrimary == isPrimary)
            .ToListAsync(cancellationToken);
        if(professionsList.Count == 1)
        {
            professionsReturn.Add(professionsList[0]);
            professionsReturn.Add(professions.Where(p => p.Id != professionsList[0].Id).OrderBy(_ => Random.Shared.Next()).Distinct().FirstOrDefault()!);
        } else
        {
            var count = Random.Shared.Next(0, 3);
            professionsReturn.AddRange(professions.OrderBy(_ => Random.Shared.Next()).Take(count).Distinct());
        }

        return professionsReturn;
    }
}