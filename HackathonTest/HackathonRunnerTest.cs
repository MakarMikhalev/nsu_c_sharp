using HackathonContract.Model;
using HackathonHrDirector;
using HackathonHrManager;
using HackathonRunner;
using HackathonStrategy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace HackathonTest;

[TestClass]
public class HackathonRunnerTest
{
    [Test]
    public void StartHackathon_ExecutesMultipleIterationsAndPrintsResults()
    {
        var juniors = GenerateJuniors();
        var teamLeads = GenerateTeamLeads();

        var wishlistParticipants = GenerateWishlistParticipants();
        var hackathon = new Mock<Hackathon>(MockBehavior.Strict);
        hackathon.Setup(h => h.Start(juniors, teamLeads)).Returns(wishlistParticipants);
        
        var hackathonRunner = new HackathonEveryone.HackathonRunner(
            new HrManager(new TeamBuildingStrategy()),
            new HrDirector(),
            hackathon.Object
        );

        var result = hackathonRunner.Run(juniors, teamLeads);
        Assert.AreEqual(1.3333333333333333, result);
    }

    private List<Employee> GenerateJuniors()
    {
        return
        [
            new Employee(1, "Junior-1"),
            new Employee(2, "Junior-2")
        ];
    }

    private List<Employee> GenerateTeamLeads()
    {
        return new List<Employee>
        {
            new(1, "TeamLead-1"),
            new(2, "TeamLead-2")
        };
    }

    private static WishlistParticipants GenerateWishlistParticipants()
    {
        return new WishlistParticipants(
            new List<Wishlist>
            {
                new(1, [2, 1]),
                new(2, [2, 1])
            },
            new List<Wishlist>
            {
                new(1, [2, 1]),
                new(2, [2, 1])
            }
        );
    }
}