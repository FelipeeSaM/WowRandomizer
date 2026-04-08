namespace WowRandomizer.Api.Entities;

public class RaceClass
{
    public int RaceId { get; set; }
    public Race Race { get; set; } = null!;
    public int ClassId { get; set; }
    public GameClass Class { get; set; } = null!;
}
