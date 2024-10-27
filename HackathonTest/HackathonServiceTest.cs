using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using HackathonContract.Model;
using HackathonDatabase.service;

namespace HackathonDatabase.Tests
{
    public class HackathonServiceTests
    {
        private ApplicationDbContext _context;
        private HackathonService _hackathonService;

        [SetUp]
        public void Setup()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();

            _hackathonService = new HackathonService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void SaveHackathon_ShouldSaveToDatabase()
        {
            // Arrange
            var harmonicMean = 4.5;
            var hackathonMetaInfo = new HackathonMetaInfo(
                TeamLeadsWishlists: new List<Wishlist>
                {
                    new Wishlist(1, new[] { 2, 3 })
                },
                JuniorsWishlists: new List<Wishlist>
                {
                    new Wishlist(2, new[] { 1 })
                },
                Teams: new List<Team>
                {
                    new Team(new Employee(1, "Lead1"), new Employee(2, "Junior1"))
                }
            );

            // Act
            _hackathonService.SaveHackathon(harmonicMean, hackathonMetaInfo);

            // Assert
            var savedHackathon = _context.HackathonEntities
                .Include(h => h.Teams)
                .Include(h => h.Wishlists)
                .FirstOrDefault();

            Assert.IsNotNull(savedHackathon);
            Assert.AreEqual(harmonicMean, savedHackathon.HarmonicMean);
            Assert.AreEqual(1, savedHackathon.Teams.Count);
            Assert.AreEqual(2, savedHackathon.Wishlists.Count);
        }

        [Test]
        public void GetHackathon_ShouldReturnCorrectDataFromDatabase()
        {
            // Arrange
            var harmonicMean = 4.5;
            var hackathonMetaInfo = new HackathonMetaInfo(
                TeamLeadsWishlists: new List<Wishlist>
                {
                    new Wishlist(1, new[] { 2, 3 })
                },
                JuniorsWishlists: new List<Wishlist>
                {
                    new Wishlist(2, new[] { 1 })
                },
                Teams: new List<Team>
                {
                    new Team(new Employee(1, "Lead1"), new Employee(2, "Junior1"))
                }
            );
            _hackathonService.SaveHackathon(harmonicMean, hackathonMetaInfo);

            // Act
            var savedHackathon = _context.HackathonEntities
                .Include(h => h.Teams)
                .Include(h => h.Wishlists)
                .FirstOrDefault();

            // Assert
            Assert.IsNotNull(savedHackathon);
            Assert.AreEqual(4.5, savedHackathon.HarmonicMean);
            Assert.AreEqual(1, savedHackathon.Teams.Count);
            Assert.AreEqual(2, savedHackathon.Wishlists.Count);
        }
    }
}