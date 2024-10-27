using HackathonContract.Model;
using HackathonDatabase.model;
using HackathonDatabase.service;
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
        var juniors = generateJuniorsEntities();
        var teamLeads = GenerateTeamLeadsEntities();

        var wishlistParticipants = GenerateWishlistParticipants();
        var hackathon = new Mock<Hackathon>(MockBehavior.Strict);
        var hackathonService = new Mock<HackathonService>(MockBehavior.Strict);
        hackathon.Setup(h => h.Start(juniors, teamLeads)).Returns(wishlistParticipants);
        hackathonService
            .Setup(h => h.SaveHackathon(It.IsAny<double>(), It.IsAny<HackathonMetaInfo>()))
            .Verifiable();
        var employeeService = new Mock<EmployeeService>(MockBehavior.Strict);

        employeeService
            .Setup(h =>
                h.SaveEmployeesByTypeAsync(It.IsAny<ICollection<Employee>>(),
                    It.IsAny<EmployeeType>()))
            .Verifiable();

        employeeService
            .Setup(h =>
                h.GetEmployeeByType(EmployeeType.JUNIOR))
            .Returns(juniors);

        employeeService
            .Setup(h =>
                h.GetEmployeeByType(EmployeeType.TEAM_LEAD))
            .Returns(teamLeads);

        var hackathonRunner = new HackathonEveryone.HackathonRunner(
            new HrManager(new TeamBuildingStrategy()),
            new HrDirector(),
            hackathon.Object,
            hackathonService.Object,
            employeeService.Object
        );

        var result = hackathonRunner.Run(GenerateJuniors(), GenerateTeamLeads());
        Assert.AreEqual(1.3333333333333333, result);
    }

    private List<EmployeeEntity> generateJuniorsEntities()
    {
        return new List<EmployeeEntity>
        {
            new()
            {
                Id = 1,
                Name = "Juniors-1"
            },
            new()
            {
                Id = 2,
                Name = "Juniors-2"
            },
        };
    }

    private List<Employee> GenerateJuniors()
    {
        return new List<Employee>
        {
            new(1, "Juniors-1"),
            new(2, "Juniors-2")
        };
    }

    private List<Employee> GenerateTeamLeads()
    {
        return new List<Employee>
        {
            new(1, "TeamLead-1"),
            new(2, "TeamLead-2")
        };
    }

    private List<EmployeeEntity> GenerateTeamLeadsEntities()
    {
        return new List<EmployeeEntity>
        {
            new()
            {
                Id = 1,
                Name = "TeamLead-1"
            },
            new()
            {
                Id = 2,
                Name = "TeamLead-2"
            },
        };
    }

    private static WishlistParticipants GenerateWishlistParticipants()
    {
        return new WishlistParticipants(
            new List<Wishlist>
            {
                new(1, DesiredEmployees),
                new(2, DesiredEmployees)
            },
            new List<Wishlist>
            {
                new(1, DesiredEmployees),
                new(2, DesiredEmployees)
            }
        );
    }

    private static readonly int[] DesiredEmployees = { 2, 1 };
}