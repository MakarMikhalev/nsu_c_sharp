using HackathonDatabase.model;
using HackathonDatabase.service;
using HackathonRunner;

namespace HackathonEveryone;

using HackathonContract.Model;
using HackathonHrDirector;
using HackathonHrManager;

public class HackathonRunner
{
    private readonly HrManager _hrManager;
    private readonly HrDirector _hrDirector;
    private readonly Hackathon _hackathon;
    private readonly HackathonService _hackathonService;
    private readonly EmployeeService _employeeService;

    public HackathonRunner(
        HrManager hrManager,
        HrDirector hrDirector,
        Hackathon hackathon,
        HackathonService hackathonService,
        EmployeeService employeeService)
    {
        _hrManager = hrManager;
        _hrDirector = hrDirector;
        _hackathon = hackathon;
        _hackathonService = hackathonService;
        _employeeService = employeeService;
    }

    public double Run(List<Employee> juniors, List<Employee> teamLeads)
    {
        _employeeService.SaveEmployeesByTypeAsync(juniors, EmployeeType.JUNIOR);
        var jEntity = _employeeService.GetEmployeeByType(EmployeeType.JUNIOR);
        
        _employeeService.SaveEmployeesByTypeAsync(juniors, EmployeeType.TEAM_LEAD);
        var tEntity = _employeeService.GetEmployeeByType(EmployeeType.TEAM_LEAD);

        var wishlistParticipants = _hackathon.Start(jEntity, tEntity);
        var teams = _hrManager.OrganizeHackathon(juniors, teamLeads, wishlistParticipants);
        Console.WriteLine("Mean harmonic " + _hrDirector.CalculateMeanHarmonic(teams));
        var harmonicMean = _hrDirector.CalculateMeanHarmonic(teams); 
        _hackathonService.SaveHackathon(harmonicMean, teams);
        return harmonicMean;
    }
}