namespace WowRandomizer.Api.Entities;

public class Faction
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<FactionRace> FactionRaces { get; set; } = [];
}
