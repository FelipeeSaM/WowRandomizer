namespace WowRandomizer.Api.Features.Character;

public record GenerateCharacterResult(
    Guid Id,
    string Faction,
    string Race,
    string Class,
    string Gender,
    string? Profession1,
    string? Profession2,
    string? SubProfession1,
    string? SubProfession2,
    DateTime GeneratedAt
);
