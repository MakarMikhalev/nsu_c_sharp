using HackathonEveryone.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace HackathonEveryone;

public class HackathonWorker(
    HackathonRunner hackathonRunner,
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

    public void StartHackathon()
    {
        try
        {
            var juniorFile = configuration["HackathonSettings:JuniorFile"];
            var teamLeadFile = configuration["HackathonSettings:TeamLeadFile"];
            var countIteration = int.Parse(configuration["HackathonSettings:CountIteration"] ?? string.Empty);
            
            List<double> harmonicsResults = [];
            var juniors = ParseCsv.RunAsync(juniorFile);
            var teamLeads = ParseCsv.RunAsync(teamLeadFile);

            for (var i = 1; i <= countIteration; ++i)
            {
                var harmonic = hackathonRunner.Run(juniors, teamLeads);
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