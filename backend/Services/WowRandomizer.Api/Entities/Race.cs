namespace WowRandomizer.Api.Entities;

public class Race
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsNeutral { get; set; }
    public ICollection<FactionRace> FactionRaces { get; set; } = [];
    public ICollection<RaceClass> RaceClasses { get; set; } = [];
}
