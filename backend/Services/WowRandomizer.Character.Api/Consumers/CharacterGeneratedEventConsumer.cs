using BuildingBlocks.Messaging.Events;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using WowRandomizer.Character.Api.Database;

namespace WowRandomizer.Character.Api.Consumers;

public class CharacterGeneratedEventConsumer(CharacterDbContext db, IDistributedCache cache)
    : IConsumer<CharacterGeneratedEvent>
{
    public async Task Consume(ConsumeContext<CharacterGeneratedEvent> context)
    {
        var character = new CharacterEntity
        {
            Id            = context.Message.Id,
            Faction       = context.Message.Faction,
            Race          = context.Message.Race,
            Class         = context.Message.Class,
            Gender        = context.Message.Gender,
            Profession1   = context.Message.Profession1,
            Profession2   = context.Message.Profession2,
            SubProfession1 = context.Message.SubProfession1,
            SubProfession2 = context.Message.SubProfession2,
            GeneratedAt   = context.Message.GeneratedAt,
            SavedAt       = DateTime.UtcNow
        };

        db.Characters.Add(character);
        await db.SaveChangesAsync(context.CancellationToken);

        await cache.RemoveAsync("CharactersList", context.CancellationToken);
    }
}
