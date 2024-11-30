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
    private readonly IHackathonService _hackathonService;
    private readonly IEmployeeService _employeeService;

    public HackathonRunner(
        HrManager hrManager,
        HrDirector hrDirector,
        Hackathon hackathon,
        IHackathonService hackathonService,
        IEmployeeService employeeService)
    {
        _hrManager = hrManager;
        _hrDirector = hrDirector;
        _hackathon = hackathon;
        _hackathonService = hackathonService;
        _employeeService = employeeService;
    }

    public double Run(List<Employee> juniors, List<Employee> teamLeads)
    {
        _employeeService.SaveEmployeesByTypeAsync(juniors, EmployeeType.Junior);
        _employeeService.SaveEmployeesByTypeAsync(juniors, EmployeeType.TeamLead);

        var wishlistParticipants = _hackathon.Start(juniors, teamLeads);
        var teams = _hrManager.OrganizeHackathon(juniors, teamLeads, wishlistParticipants);
        var harmonicMean = _hrDirector.CalculateMeanHarmonic(teams);
        var hackathonId = _hackathonService.SaveHackathon(harmonicMean, teams);
        PrintHackathonResult(hackathonId);
        PrintHarmonicMeanHackathon();
        return harmonicMean;
    }

    private void PrintHackathonResult(int hackathonId)
    {
        var hackathonEntity = _hackathonService.GetHackathonById(hackathonId);
        if (hackathonEntity == null)
        {
            Console.WriteLine($"Hackathon with ID {hackathonId} not found.");
            return;
        }

        Console.WriteLine("_________________________________________________________");
        Console.WriteLine($"Hackathon Results (ID: {hackathonId}):");
        Console.WriteLine($"Harmonic mean (from database): {hackathonEntity.HarmonicMean}");

        Console.WriteLine("Formed Teams:");
        foreach (var team in hackathonEntity.Teams)
        {
            Console.WriteLine($" - Team Lead ID: {team.TeamLeadId}, Junior ID: {team.JuniorId}");
        }

        Console.WriteLine("_________________________________________________________");

        Console.WriteLine("Participant Wishlists:");
        foreach (var wishlist in hackathonEntity.Wishlists)
        {
            Console.WriteLine(
                $" - Participant ID: {wishlist.EmployeeId}, Desired Colleagues: {string.Join(", ", wishlist.DesiredEmployeeIds)}");
        }
    }

    private void PrintHarmonicMeanHackathon()
    {
        Console.WriteLine("_________________________________________________________");
        Console.WriteLine(
            $"Average Harmonic Mean for all hackathons: {_hackathonService.CalculateAverageHarmonicMean()}");
        Console.WriteLine("_________________________________________________________");
    }
}