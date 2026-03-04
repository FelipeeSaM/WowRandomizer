namespace WowRandomizer.Api.Entities;

public class FactionRace
{
    public int FactionId { get; set; }
    public Faction Faction { get; set; } = null!;
    public int RaceId { get; set; }
    public Race Race { get; set; } = null!;
}
