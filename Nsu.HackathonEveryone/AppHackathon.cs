using HackathonEveryone.Model.Employee;
using HackathonEveryone.Utils;

namespace HackathonEveryone;

public class AppHackathon
{
    private const string JuniorFile = "./Juniors20.csv";
    private const string TeamLeadFile = "./Teamleads20.csv";
    private const int СountInteration = 1000;

    private readonly HrManager _hrManager = new();
    private readonly HrDirector _hrDirector = new();

    public void Run()
    {
        try
        {
            List<double> harmonicsResults = [];

            for (var i = 0; i < СountInteration; ++i)
            {
                var juniors = ParseCsv.RunAsync(JuniorFile);
                var teamLeads = ParseCsv.RunAsync(TeamLeadFile);

                var teams = _hrManager.OrganizeHackathon(juniors, teamLeads);
                var harmonic = _hrDirector.CalculateMeanHarmonic(teams);
                harmonicsResults.Add(harmonic);
                Console.WriteLine("Mean harmonic " + _hrDirector.CalculateMeanHarmonic(teams));
            }

            PrintHarmonicResult(harmonicsResults);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Run: message exception: {ex.Message}");
        }
    }

    private static void PrintHarmonicResult(List<double> harmonicsResults)
    {
        Console.WriteLine(
            "______________________________________________________________________________________________");
        Console.WriteLine(
            $"Harmonic mean = {СountInteration / harmonicsResults.Sum()} for {СountInteration} iteration(s)");
    }
}