namespace BuildingBlocks.Messaging.Events;

public record CharacterGeneratedEvent(
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
