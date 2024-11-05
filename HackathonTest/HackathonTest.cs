using HackathonRunner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using CollectionAssert = Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert;

namespace HackathonTest;

[TestClass]
public class HackathonTest
{
    private Hackathon _hackathon;
    private IEnumerable<Employee> _juniors;
    private IEnumerable<Employee> _seniors;

    [SetUp]
    public void Setup()
    {
        _hackathon = new Hackathon();
        _juniors = ModelFactory.GenerateEmployees(3, "Junior");
        _seniors = ModelFactory.GenerateEmployees(3, "Senior");
    }

    [Test]
    public void Success_CreateWishlist_ShouldCreateWishlistsForJuniorsAndSeniors()
    {
        // Arrange & Act
        var wishlist = _hackathon.Start(_juniors, _seniors);

        // Assert
        AssertWishlist(wishlist);
    }

    [Test]
    public void Success_CreateWishlistDesiredEmployees_ShouldContainAllTeamLeadsInJuniorWishlists()
    {
        // Arrange & Act
        var wishlist = _hackathon.Start(_juniors, _seniors);

        // Assert
        AssertDesiredEmployees(wishlist);
    }

    private void AssertWishlist(WishlistParticipants wishlist)
    {
        Assert.AreEqual(3, wishlist.TeamLeadsWishlists.Count());
        Assert.AreEqual(3, wishlist.JuniorsWishlists.Count());

        var teamLeadsWishlists = wishlist.TeamLeadsWishlists.Select(w => w.EmployeeId);
        var teamLeads = _seniors.Select(e => e.Id);
        Assert.IsTrue(teamLeadsWishlists.SequenceEqual(teamLeads));

        var juniorsWishlists = wishlist.JuniorsWishlists.Select(w => w.EmployeeId);
        var juniors = _juniors.Select(e => e.Id);
        Assert.IsTrue(juniorsWishlists.SequenceEqual(juniors));
    }

    private void AssertDesiredEmployees(WishlistParticipants wishlist)
    {
        foreach (var juniorWishlist in wishlist.JuniorsWishlists)
        {
            var desiredEmployees = juniorWishlist.DesiredEmployees;
            var teamLeadsIds = _seniors.Select(e => e.Id).Distinct().ToArray();

            Assert.AreEqual(teamLeadsIds.Length,
                desiredEmployees.Length,
                "Количество team lead-ов должно соответсвовать количеству juniors");

            CollectionAssert.AreEquivalent(teamLeadsIds,
                desiredEmployees,
                "Все участники должны присутствовать в списке juniors");
        }
    }
}