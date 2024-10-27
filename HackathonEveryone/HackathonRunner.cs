using HackathonRunner;

namespace HackathonEveryone;

using HackathonContract.Model;
using HackathonHrDirector;
using HackathonHrManager;

public class HackathonRunner(
    HrManager hrManager,
    HrDirector hrDirector,
    Hackathon hackathon)
{
    public double Run(List<Employee> juniors, List<Employee> teamLeads)
    {
        var wishlistParticipants = hackathon.Start(juniors, teamLeads);
        var teams = hrManager.OrganizeHackathon(juniors, teamLeads, wishlistParticipants);
        Console.WriteLine("Mean harmonic " + hrDirector.CalculateMeanHarmonic(teams));
        return hrDirector.CalculateMeanHarmonic(teams);
    }
}