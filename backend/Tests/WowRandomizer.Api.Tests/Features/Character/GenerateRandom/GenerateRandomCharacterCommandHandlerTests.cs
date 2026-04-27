using MassTransit;
using Microsoft.EntityFrameworkCore;
using WowRandomizer.Api.Features.Character.GenerateRandom;
using WowRandomizer.Api.Database;
using NSubstitute;
using WowRandomizer.Api.Features.Professions.GetProfessions;

namespace WowRandomizer.Api.Tests.Features.Character.GenerateRandom
{
    [TestFixture]
    public class GenerateRandomCharacterCommandHandlerTests
    {
        #region DI
        private AppDbContext _db = null!;
        private IPublishEndpoint _publishEndpoint = null!;
        private GenerateRandomCharacterCommandHandler _handler = null!;
        private GetProfessionsQueryHandler _handlerProfessions = null!;
        #endregion

        #region Test Setup
        [SetUp]
        public async Task Setup()
        {
            var connString = "Server=localhost,1433;Database=WowRandomizerDb;User Id=sa;Password=Strong!Pass1;Encrypt=False;TrustServerCertificate=True";

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connString)
                .Options;

            _db = new AppDbContext(options);
            await _db.Database.EnsureCreatedAsync();

            _publishEndpoint = Substitute.For<IPublishEndpoint>();
            _handler = new GenerateRandomCharacterCommandHandler(_db, _publishEndpoint);
            _handlerProfessions = new GetProfessionsQueryHandler(_db);
        }

        [TearDown]
        public async Task TearDown()
        {
            await _db.DisposeAsync();
        }
        #endregion

        #region Tests
        [Test]
        public async Task Handle_ReturnsValidCharacter()
        {
            var command = new GenerateRandomCharacterCommand();
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(result.Faction, Is.Not.Null.Or.Empty);
            Assert.That(result.Race, Is.Not.Null.Or.Empty);
            Assert.That(result.Class, Is.Not.Null.Or.Empty);
            Assert.That(result.Gender, Is.Not.Null.Or.Empty);
        }

        [Test]
        public async Task Handle_ReturnsCharacterWithValidProfessions()
        {
            var command = new GenerateRandomCharacterCommand();
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.That(result.Profession1, Is.Null.Or.Not.Empty);
            Assert.That(result.Profession2, Is.Null.Or.Not.Empty);
            Assert.That(result.SubProfession1, Is.Null.Or.Not.Empty);
            Assert.That(result.SubProfession2, Is.Null.Or.Not.Empty);
        }

        [TestCase("Blacksmithing")]
        [TestCase("Alchemy")]
        public async Task Handle_VerifyProfessionsExistsForPrimary_ReturnsTrue(string professionName)
        {
            var result = await _handlerProfessions.Handle(new GetProfessionsQuery(true), CancellationToken.None);
            Assert.That(result.Professions, Is.Not.Empty);
            Assert.That(result.Professions.Any(c => c.Name == professionName), Is.True, $"Expected to find profession '{professionName}' in primary professions.");
        }

        [TestCase("Fishing")]
        [TestCase("Cooking")]
        [TestCase("Archaeology")]
        public async Task Handle_VerifyProfessionsExistsSecondary_ReturnsTrue(string professionName) {
            var result = await _handlerProfessions.Handle(new GetProfessionsQuery(false), CancellationToken.None);
            Assert.That(result.Professions, Is.Not.Empty);
            Assert.That(result.Professions.Any(c => c.Name == professionName), Is.True, $"Expected to find profession '{professionName}' in secondary professions.");
        }
        #endregion
    }
}