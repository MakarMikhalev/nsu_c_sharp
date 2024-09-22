using HackathonEveryone.Utils;
using HackathonHrDirector;
using HackathonHrManager;
using HackathonRunner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace HackathonEveryone;

public class HackathonWorker(
    HrManager hrManager,
    HrDirector hrDirector,
    Hackathon hackathon,
    IConfiguration configuration)
    : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        StartHackathon();
        return Task.CompletedTask;
    }


    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private void StartHackathon()
    {
        try
        {
            var juniorFile = configuration["HackathonSettings:JuniorFile"];
            var teamLeadFile = configuration["HackathonSettings:TeamLeadFile"];
            var countIteration =
                int.Parse(configuration["HackathonSettings:CountIteration"] ?? string.Empty);
            
            List<double> harmonicsResults = [];
            var juniors = ParseCsv.RunAsync(juniorFile);
            var teamLeads = ParseCsv.RunAsync(teamLeadFile);

            for (var i = 1; i <= countIteration; ++i)
            {
                var wishlistParticipants = hackathon.Start(juniors, teamLeads);
                var teams = hrManager.OrganizeHackathon(juniors, teamLeads, wishlistParticipants);
                var harmonic = hrDirector.CalculateMeanHarmonic(teams);
                Console.WriteLine("Mean harmonic " + hrDirector.CalculateMeanHarmonic(teams));
                harmonicsResults.Add(harmonic);
                PrintHarmonicResult(harmonicsResults, i);
            }

            PrintHarmonicResult(harmonicsResults, countIteration);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Run: message exception: {ex.Message}");
        }
    }

    private static void PrintHarmonicResult(List<double> harmonicsResults, int countIteration)
    {
        Console.WriteLine("_________________________________________________________");
        Console.WriteLine(
            $"Harmonic mean = {harmonicsResults.Sum() / countIteration} for {countIteration} iteration(s)");
    }
}