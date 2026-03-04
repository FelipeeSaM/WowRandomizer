using Microsoft.EntityFrameworkCore;
using WowRandomizer.Api.Entities;

namespace WowRandomizer.Api.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Faction> Factions => Set<Faction>();
    public DbSet<Race> Races => Set<Race>();
    public DbSet<GameClass> Classes => Set<GameClass>();
    public DbSet<Profession> Professions => Set<Profession>();
    public DbSet<FactionRace> FactionRaces => Set<FactionRace>();
    public DbSet<RaceClass> RaceClasses => Set<RaceClass>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FactionRace>(entity =>
        {
            entity.HasKey(fr => new { fr.FactionId, fr.RaceId });
            entity.HasOne(fr => fr.Faction)
                  .WithMany(f => f.FactionRaces)
                  .HasForeignKey(fr => fr.FactionId);
            entity.HasOne(fr => fr.Race)
                  .WithMany(r => r.FactionRaces)
                  .HasForeignKey(fr => fr.RaceId);
        });

        modelBuilder.Entity<RaceClass>(entity =>
        {
            entity.HasKey(rc => new { rc.RaceId, rc.ClassId });
            entity.HasOne(rc => rc.Race)
                  .WithMany(r => r.RaceClasses)
                  .HasForeignKey(rc => rc.RaceId);
            entity.HasOne(rc => rc.Class)
                  .WithMany(c => c.RaceClasses)
                  .HasForeignKey(rc => rc.ClassId);
        });
    }
}
