using BuildingBlocks.Messaging.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using WowRandomizer.Api.Database;
using WowRandomizer.Api.Entities;
using WowRandomizer.Api.Features.Character;
using WowRandomizer.Api.Features.Character.GenerateCustom;

namespace WowRandomizer.Api.Tests.Features.Character.GenerateCustom;

[TestFixture]
public class GenerateCustomCharacterCommandHandlerTests
{
    private AppDbContext _db = null!;
    private IPublishEndpoint _publishEndpoint = null!;
    private GenerateCustomCharacterCommandHandler _handler = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _db = new AppDbContext(options);
        _publishEndpoint = Substitute.For<IPublishEndpoint>();
        _handler = new GenerateCustomCharacterCommandHandler(_db, _publishEndpoint);

        SeedDatabase();
    }

    [TearDown]
    public void TearDown() => _db.Dispose();

    // ─── Testes de filtragem por Faction / Race / Class ────────────────────────

    [Test]
    public async Task Handle_WithSpecificFaction_ResultHasThatFaction()
    {
        var command = new GenerateCustomCharacterCommand("Alliance", null, null, null, null);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.That(result.Faction, Is.EqualTo("Alliance"));
    }

    [Test]
    public async Task Handle_WithSpecificRace_ResultHasThatRace()
    {
        var command = new GenerateCustomCharacterCommand(null, "Human", null, null, null);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.That(result.Race, Is.EqualTo("Human"));
    }

    [Test]
    public async Task Handle_WithSpecificClass_ResultHasThatClass()
    {
        var command = new GenerateCustomCharacterCommand(null, null, "Warrior", null, null);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.That(result.Class, Is.EqualTo("Warrior"));
    }

    [Test]
    public async Task Handle_WithAllThreeConstraints_ResultMatchesAllThree()
    {
        var command = new GenerateCustomCharacterCommand("Alliance", "Human", "Mage", null, null);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result.Faction, Is.EqualTo("Alliance"));
            Assert.That(result.Race, Is.EqualTo("Human"));
            Assert.That(result.Class, Is.EqualTo("Mage"));
        });
    }

    // ─── Testes de combinação inválida ─────────────────────────────────────────

    [Test]
    public void Handle_WithIncompatibleFactionAndRace_ThrowsArgumentException()
    {
        // Orc pertence à Horde, não à Alliance
        var command = new GenerateCustomCharacterCommand("Alliance", "Orc", null, null, null);

        Assert.ThrowsAsync<ArgumentException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    [Test]
    public void Handle_WithRaceIncompatibleWithClass_ThrowsArgumentException()
    {
        // Night Elf só pode ser Hunter no banco de teste; Mage não é compatível
        var command = new GenerateCustomCharacterCommand(null, "Night Elf", "Mage", null, null);

        Assert.ThrowsAsync<ArgumentException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    // ─── Testes de resultado genérico ──────────────────────────────────────────

    [Test]
    public async Task Handle_WithNoConstraints_ReturnsValidCharacter()
    {
        var command = new GenerateCustomCharacterCommand(null, null, null, null, null);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(result.Faction, Is.Not.Null.And.Not.Empty);
            Assert.That(result.Race, Is.Not.Null.And.Not.Empty);
            Assert.That(result.Class, Is.Not.Null.And.Not.Empty);
            Assert.That(result.Gender, Is.AnyOf("Male", "Female"));
            Assert.That(result.GeneratedAt, Is.Not.EqualTo(default(DateTime)));
        });
    }

    [Test]
    public async Task Handle_CalledTwice_GeneratesDistinctIds()
    {
        var command = new GenerateCustomCharacterCommand(null, null, null, null, null);

        var result1 = await _handler.Handle(command, CancellationToken.None);
        var result2 = await _handler.Handle(command, CancellationToken.None);

        Assert.That(result1.Id, Is.Not.EqualTo(result2.Id));
    }

    // ─── Testes de publicação de evento ────────────────────────────────────────

    [Test]
    public async Task Handle_Always_PublishesCharacterGeneratedEvent()
    {
        var command = new GenerateCustomCharacterCommand("Alliance", "Human", "Warrior", null, null);

        await _handler.Handle(command, CancellationToken.None);

        await _publishEndpoint.Received(1)
            .Publish(Arg.Any<CharacterGeneratedEvent>(), Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Handle_Always_PublishedEventContainsCorrectData()
    {
        var command = new GenerateCustomCharacterCommand("Alliance", "Human", "Warrior", null, null);

        var result = await _handler.Handle(command, CancellationToken.None);

        await _publishEndpoint.Received(1).Publish(
            Arg.Is<CharacterGeneratedEvent>(e =>
                e.Id == result.Id &&
                e.Faction == result.Faction &&
                e.Race == result.Race &&
                e.Class == result.Class &&
                e.Gender == result.Gender),
            Arg.Any<CancellationToken>());
    }

    // ─── Testes de profissões primárias ────────────────────────────────────────

    [Test]
    public async Task Handle_WithTwoPrimaryProfessions_ReturnsBothInResult()
    {
        var profs = new List<Profession>
        {
            new() { Id = 1, Name = "Mining", IsPrimary = true },
            new() { Id = 2, Name = "Blacksmithing", IsPrimary = true },
        };
        var command = new GenerateCustomCharacterCommand(null, null, null, profs, null);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result.Profession1, Is.EqualTo("Mining"));
            Assert.That(result.Profession2, Is.EqualTo("Blacksmithing"));
        });
    }

    [Test]
    public async Task Handle_WithOnePrimaryProfession_ReturnsTwoProfessions()
    {
        var profs = new List<Profession>
        {
            new() { Id = 1, Name = "Mining", IsPrimary = true },
        };
        var command = new GenerateCustomCharacterCommand(null, null, null, profs, null);

        var result = await _handler.Handle(command, CancellationToken.None);

        // A profissão fornecida deve aparecer; a segunda é preenchida aleatoriamente
        Assert.Multiple(() =>
        {
            Assert.That(result.Profession1, Is.EqualTo("Mining"));
            Assert.That(result.Profession2, Is.Not.Null);
            Assert.That(result.Profession2, Is.Not.EqualTo("Mining"));
        });
    }

    [Test]
    public async Task Handle_WithNoPrimaryProfessions_ReturnsBetweenZeroAndTwo()
    {
        var command = new GenerateCustomCharacterCommand(null, null, null, null, null);

        var result = await _handler.Handle(command, CancellationToken.None);

        var profCount = new[] { result.Profession1, result.Profession2 }.Count(p => p is not null);
        Assert.That(profCount, Is.InRange(0, 2));
    }

    // ─── Testes de profissões secundárias (sub-profissões) ─────────────────────

    [Test]
    public async Task Handle_WithTwoSubProfessions_ReturnsBothInResult()
    {
        var subs = new List<Profession>
        {
            new() { Id = 4, Name = "First Aid", IsPrimary = false },
            new() { Id = 5, Name = "Cooking", IsPrimary = false },
        };
        var command = new GenerateCustomCharacterCommand(null, null, null, null, subs);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result.SubProfession1, Is.EqualTo("First Aid"));
            Assert.That(result.SubProfession2, Is.EqualTo("Cooking"));
        });
    }

    // ─── Helpers ───────────────────────────────────────────────────────────────

    private void SeedDatabase()
    {
        var alliance = new Faction { Id = 1, Name = "Alliance" };
        var horde    = new Faction { Id = 2, Name = "Horde" };

        var human    = new Race { Id = 1, Name = "Human" };
        var orc      = new Race { Id = 2, Name = "Orc" };
        var nightElf = new Race { Id = 3, Name = "Night Elf" };

        var warrior = new GameClass { Id = 1, Name = "Warrior" };
        var mage    = new GameClass { Id = 2, Name = "Mage" };
        var hunter  = new GameClass { Id = 3, Name = "Hunter" };

        _db.Factions.AddRange(alliance, horde);
        _db.Races.AddRange(human, orc, nightElf);
        _db.Classes.AddRange(warrior, mage, hunter);

        _db.FactionRaces.AddRange(
            new FactionRace { FactionId = 1, RaceId = 1 },   // Alliance ↔ Human
            new FactionRace { FactionId = 1, RaceId = 3 },   // Alliance ↔ Night Elf
            new FactionRace { FactionId = 2, RaceId = 2 });  // Horde ↔ Orc

        _db.RaceClasses.AddRange(
            new RaceClass { RaceId = 1, ClassId = 1 },   // Human → Warrior
            new RaceClass { RaceId = 1, ClassId = 2 },   // Human → Mage
            new RaceClass { RaceId = 2, ClassId = 1 },   // Orc → Warrior
            new RaceClass { RaceId = 3, ClassId = 3 });  // Night Elf → Hunter

        _db.Professions.AddRange(
            new Profession { Id = 1, Name = "Mining",         IsPrimary = true  },
            new Profession { Id = 2, Name = "Blacksmithing",  IsPrimary = true  },
            new Profession { Id = 3, Name = "Herbalism",      IsPrimary = true  },
            new Profession { Id = 4, Name = "First Aid",      IsPrimary = false },
            new Profession { Id = 5, Name = "Cooking",        IsPrimary = false });

        _db.SaveChanges();
    }
}
