using HackathonContract.Model;
using HackathonDatabase.model;
using HackathonDatabase.service;

namespace HackathonHrDirector;

public class HrDirectorService(
    EmployeeService employeeService,
    HackathonService hackathonService,
    HrDirector hrDirector)
{
    public void SummingUp(HackathonResult hackathonResult)
    {
        employeeService.SaveEmployeesByTypeAsync(
            hackathonResult.JuniorEmployees,
            EmployeeType.Junior
        );
        employeeService.SaveEmployeesByTypeAsync(
            hackathonResult.JuniorEmployees,
            EmployeeType.TeamLead
        );

        var harmonicMean = hrDirector.CalculateMeanHarmonic(hackathonResult.HackathonMetaInfo);

        Console.WriteLine("Mean harmonic " + harmonicMean);
        hackathonService.SaveHackathon(harmonicMean, hackathonResult.HackathonMetaInfo);
    }
}