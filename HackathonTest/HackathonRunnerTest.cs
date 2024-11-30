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
 [TestMethod, TestCaseSource(nameof(StartHackathonTestCases))]
    public void  Success_StartHackathon_CalculatesMeanHarmonic(
        List<EmployeeEntity> jEmployeeEntities,
        List<EmployeeEntity> tEmployeeEntities,
        List<Employee> jEmployees,
        List<Employee> tEmployees,
        WishlistParticipants wishlistParticipants,
        double expectedResult)
    {
        var hackathon = CreateMockHackathon(jEmployees, tEmployees, wishlistParticipants);
        var hackathonService = CreateMockHackathonService();
        var employeeService = CreateMockEmployeeService();

        var hackathonRunner = CreateHackathonRunner(hackathon, hackathonService, employeeService);

        var result = hackathonRunner.Run(jEmployees, tEmployees);

        Assert.AreEqual(expectedResult, result, 0.001);
    }

    private Mock<Hackathon> CreateMockHackathon(
        List<Employee> jEmployee,
        List<Employee> tEmployee,
        WishlistParticipants wishlistParticipants)
    {
        var hackathon = new Mock<Hackathon>(MockBehavior.Strict);
        hackathon.Setup(h => h.Start(jEmployee, tEmployee))
            .Returns(wishlistParticipants);
        return hackathon;
    }

    private Mock<IHackathonService> CreateMockHackathonService()
    {
        var hackathonService = new Mock<IHackathonService>(MockBehavior.Strict);
        hackathonService
            .Setup(h => h.SaveHackathon(It.IsAny<double>(), It.IsAny<HackathonMetaInfo>()))
            .Returns(1)
            .Verifiable();
        hackathonService.Setup(h => h.GetHackathonById(It.IsAny<int>()))
            .Returns((HackathonEntity)null);
        
        hackathonService.Setup(h => h.CalculateAverageHarmonicMean())
            .Returns(1.0);
        return hackathonService;
    }

    private Mock<IEmployeeService> CreateMockEmployeeService()
    {
        var employeeService = new Mock<IEmployeeService>(MockBehavior.Strict);

        employeeService.Setup(h =>
                h.SaveEmployeesByTypeAsync(It.IsAny<ICollection<Employee>>(),
                    It.IsAny<EmployeeType>()));
        return employeeService;
    }

    private HackathonEveryone.HackathonRunner CreateHackathonRunner(
        Mock<Hackathon> hackathon,
        Mock<IHackathonService> hackathonService,
        Mock<IEmployeeService> employeeService)
    {
        return new HackathonEveryone.HackathonRunner(
            new HrManager(new TeamBuildingStrategy()),
            new HrDirector(),
            hackathon.Object,
            hackathonService.Object,
            employeeService.Object
        );
    }

    private static IEnumerable<object[]> StartHackathonTestCases()
    {
        yield return new object[]
        {
            ModelFactory.GenerateEmployeeEntities(27, "Juniors").ToList(),
            ModelFactory.GenerateEmployeeEntities(27, "TeamLead").ToList(),
            ModelFactory.GenerateEmployees(27, "Juniors").ToList(),
            ModelFactory.GenerateEmployees(27, "TeamLead").ToList(),
            ModelFactory.GenerateWishlistParticipants(27),
            6.938d
        };

        yield return new object[]
        {
            ModelFactory.GenerateEmployeeEntities(4, "Juniors").ToList(),
            ModelFactory.GenerateEmployeeEntities(4, "TeamLead").ToList(),
            ModelFactory.GenerateEmployees(4, "Juniors").ToList(),
            ModelFactory.GenerateEmployees(4, "TeamLead").ToList(),
            ModelFactory.GenerateWishlistParticipants(4),
            1.92d
        };
    }
}