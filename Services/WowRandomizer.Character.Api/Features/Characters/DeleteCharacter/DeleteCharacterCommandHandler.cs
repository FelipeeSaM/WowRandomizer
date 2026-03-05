using Microsoft.EntityFrameworkCore;
using WowRandomizer.Character.Api.Database;

namespace WowRandomizer.Character.Api.Features.Characters.DeleteCharacter;

public record DeleteCharacterCommand(Guid Id) : ICommand<DeleteCharacterResult>;

public record DeleteCharacterResult(bool Success);

public class DeleteCharacterCommandHandler(CharacterDbContext db)
    : ICommandHandler<DeleteCharacterCommand, DeleteCharacterResult>
{
    public async Task<DeleteCharacterResult> Handle(DeleteCharacterCommand request, CancellationToken cancellationToken)
    {
        var character = await db.Characters
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (character is null)
            throw new KeyNotFoundException($"Character with id '{request.Id}' not found.");

        db.Characters.Remove(character);
        await db.SaveChangesAsync(cancellationToken);

        return new DeleteCharacterResult(true);
    }
}
