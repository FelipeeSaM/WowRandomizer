namespace WowRandomizer.Character.Api.Entities;

public class Character
{
    public Guid Id { get; set; }
    public string Faction { get; set; } = string.Empty;
    public string Race { get; set; } = string.Empty;
    public string Class { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string? Profession1 { get; set; }
    public string? Profession2 { get; set; }
    public string? SubProfession1 { get; set; }
    public string? SubProfession2 { get; set; }
    public DateTime GeneratedAt { get; set; }
    public DateTime SavedAt { get; set; }
}
