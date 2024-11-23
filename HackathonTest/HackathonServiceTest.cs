using HackathonContract.Model;
using HackathonDatabase;
using HackathonDatabase.model;
using HackathonDatabase.service;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace HackathonTest;

[TestFixture]
public class HackathonServiceTests
{
    private ApplicationDbContext _context;
    private HackathonService _hackathonService;

    private const double HarmonicMean = 4.5;

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
    public void  Success_SaveHackathon_ShouldSaveToDatabase()
    {
        var hackathonMetaInfo = CreateHackathonMetaInfo();
        
        _hackathonService.SaveHackathon(HarmonicMean, hackathonMetaInfo);

        var savedHackathon = GetSavedHackathon();
        AssertionResult(savedHackathon);
    }
    
    private HackathonMetaInfo CreateHackathonMetaInfo()
    {
        return new HackathonMetaInfo(
            TeamLeadsWishlists: new List<Wishlist>
            {
                new(1, new[] { 2, 3 })
            },
            JuniorsWishlists: new List<Wishlist>
            {
                new(2, new[] { 1 })
            },
            Teams: ModelFactory.GenerateTeams(1)
        );
    }

    private HackathonEntity GetSavedHackathon()
    {
        return _context.HackathonEntities
            .Include(h => h.Teams)
            .Include(h => h.Wishlists)
            .FirstOrDefault();
    }

    private void AssertionResult(HackathonEntity savedHackathon)
    {
        Assert.IsNotNull(savedHackathon);
        Assert.AreEqual(HarmonicMean, savedHackathon.HarmonicMean);
        Assert.AreEqual(1, savedHackathon.Teams.Count);
        Assert.AreEqual(2, savedHackathon.Wishlists.Count);
    }
}