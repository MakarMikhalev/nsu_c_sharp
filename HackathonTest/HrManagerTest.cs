using HackathonContract.Model;
using HackathonContract.ServiceContract;
using HackathonHrManager;
using HackathonRunner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using CollectionAssert = Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert;

namespace HackathonTest;

[TestClass]
public class HrManagerTest
{
    [Test]
    public void OrganizeHackathonTest()
    {
        var teamBuildingStrategyMock = new Mock<ITeamBuildingStrategy>();

        var predefinedTeams = new List<Team>
        {
            new(new Employee(3, "Junior-3"), new Employee(3, "Senior-3")),
            new(new Employee(2, "Junior-2"), new Employee(2, "Senior-2")),
            new(new Employee(1, "Junior-1"), new Employee(1, "Senior-1"))
        };

        teamBuildingStrategyMock.Setup(strategy =>
                strategy.BuildTeams(It.IsAny<IEnumerable<Employee>>(),
                    It.IsAny<IEnumerable<Employee>>(),
                    It.IsAny<IEnumerable<Wishlist>>(), It.IsAny<IEnumerable<Wishlist>>()))
            .Returns(predefinedTeams);

        var hrManager = new HrManager(teamBuildingStrategyMock.Object);
        var jEnumerable = GeneratorEmployer.GenerateJuniors();
        var sEnumerator = GeneratorEmployer.GenerateSeniors();
        var wishlistParticipants = new WishlistParticipants(GenerateWishlists(jEnumerable),
            GenerateWishlists(sEnumerator));

        var hackathonMetaInfo =
            hrManager.OrganizeHackathon(jEnumerable, sEnumerator, wishlistParticipants);

        Assert.AreEqual(3, hackathonMetaInfo.Teams.Count(),
            "Количество команд не совпадает с ожидаемым.");

        CollectionAssert.AreEqual(
            predefinedTeams,
            hackathonMetaInfo.Teams.ToList(),
            "Распределение команд не соответствует ожидаемому.");

        teamBuildingStrategyMock.Verify(strategy =>
                strategy.BuildTeams(
                    It.IsAny<IEnumerable<Employee>>(),
                    It.IsAny<IEnumerable<Employee>>(),
                    It.IsAny<IEnumerable<Wishlist>>(),
                    It.IsAny<IEnumerable<Wishlist>>()
                ),
            Times.Once,
            "Стратегия должна быть вызвана ровно один раз.");
    }

    private IEnumerable<Wishlist> GenerateWishlists(IEnumerable<Employee> employees)
    {
        return employees.Select(jr => new Wishlist(jr.Id, [1, 2, 3]));
    }
}