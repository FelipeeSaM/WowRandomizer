namespace WowRandomizer.Api.Entities;

public class GameClass
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<RaceClass> RaceClasses { get; set; } = [];
}
