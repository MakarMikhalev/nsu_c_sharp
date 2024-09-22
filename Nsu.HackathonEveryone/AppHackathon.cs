using HackathonEveryone.Model.Employee;
using HackathonEveryone.ServiceContract.Impl;
using HackathonEveryone.Utils;
using Microsoft.Extensions.Configuration;

namespace HackathonEveryone;

public class AppHackathon
{
    private readonly HrManager _hrManager = new(new TeamBuildingStrategy());
    private readonly HrDirector _hrDirector = new();

    public void Run(IConfiguration configuration)
    {
        try
        {
            var juniorFile = configuration["HackathonSettings:JuniorFile"];
            var teamLeadFile = configuration["HackathonSettings:TeamLeadFile"];
            var countIteration =
                int.Parse(configuration["HackathonSettings:CountIteration"] ?? string.Empty);

            List<double> harmonicsResults = [];

            for (var i = 1; i <= countIteration; ++i)
            {
                var juniors = ParseCsv.RunAsync(juniorFile);
                var teamLeads = ParseCsv.RunAsync(teamLeadFile);

                var teams = _hrManager.OrganizeHackathon(juniors, teamLeads);
                var harmonic = _hrDirector.CalculateMeanHarmonic(teams);
                harmonicsResults.Add(harmonic);
                Console.WriteLine("Mean harmonic " + _hrDirector.CalculateMeanHarmonic(teams));
                PrintHarmonicResult(harmonicsResults, i);
            }

            PrintHarmonicResult(harmonicsResults, countIteration);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Run: message exception: {ex.Message}");
        }
    }

    private static void PrintHarmonicResult(List<double> harmonicsResults, int countInteration)
    {
        Console.WriteLine("_________________________________________________________");
        Console.WriteLine(
            $"Harmonic mean = {harmonicsResults.Sum() / countInteration} for {countInteration} iteration(s)");
    }
}