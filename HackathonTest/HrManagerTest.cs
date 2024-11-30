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
    private readonly Mock<ITeamBuildingStrategy> _teamBuildingStrategyMock = new();
    private readonly List<Team> _predictionTeams = ModelFactory.GenerateTeams(3).ToList();
    private HrManager _hrManager;

    private readonly IEnumerable<Employee>
        jEmployees = ModelFactory.GenerateEmployees(3, "Junior");

    private readonly IEnumerable<Employee> sEmployees =
        ModelFactory.GenerateEmployees(3, "TeamLead");

    private WishlistParticipants _wishlistParticipants;

    [SetUp]
    public void SetUp()
    {
        _teamBuildingStrategyMock.Setup(strategy =>
                strategy.BuildTeams(It.IsAny<IEnumerable<Employee>>(),
                    It.IsAny<IEnumerable<Employee>>(),
                    It.IsAny<IEnumerable<Wishlist>>(), It.IsAny<IEnumerable<Wishlist>>()))
            .Returns(_predictionTeams);

        _hrManager = new HrManager(_teamBuildingStrategyMock.Object);

        _wishlistParticipants = new WishlistParticipants(GenerateWishlists(jEmployees),
            GenerateWishlists(sEmployees));
    }

    [Test]
    public void Success_WhenHackathonIsOrganized_ThenTeamsAreBuiltSuccessfully()
    {
        var hackathonMetaInfo = _hrManager.OrganizeHackathon(
            jEmployees,
            sEmployees,
            _wishlistParticipants
        );

        AssertHackathonResults(hackathonMetaInfo);
        VerifyStrategyWasCalledOnce();
    }

    private void AssertHackathonResults(HackathonMetaInfo hackathonMetaInfo)
    {
        Assert.AreEqual(3, hackathonMetaInfo.Teams.Count(),
            "Количество команд не совпадает с ожидаемым.");
        CollectionAssert.AreEqual(
            _predictionTeams.ToList(),
            hackathonMetaInfo.Teams.ToList(),
            "Распределение команд не соответствует ожидаемому."
        );
    }

    private void VerifyStrategyWasCalledOnce()
    {
        _teamBuildingStrategyMock.Verify(
            strategy => strategy.BuildTeams(
                It.IsAny<IEnumerable<Employee>>(),
                It.IsAny<IEnumerable<Employee>>(),
                It.IsAny<IEnumerable<Wishlist>>(),
                It.IsAny<IEnumerable<Wishlist>>()
            ),
            Times.Once,
            "Стратегия должна быть вызвана ровно один раз."
        );
    }

    private IEnumerable<Wishlist> GenerateWishlists(IEnumerable<Employee> employees)
    {
        var desiredEmployees = ModelFactory.GenerateDesiredEmployees(3);
        return employees.Select(jr => new Wishlist(jr.Id, desiredEmployees));
    }
}