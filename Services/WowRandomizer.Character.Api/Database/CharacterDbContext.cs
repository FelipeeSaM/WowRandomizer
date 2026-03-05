using Microsoft.EntityFrameworkCore;

namespace WowRandomizer.Character.Api.Database;

public class CharacterDbContext(DbContextOptions<CharacterDbContext> options) : DbContext(options)
{
    public DbSet<CharacterEntity> Characters => Set<CharacterEntity>();
}
