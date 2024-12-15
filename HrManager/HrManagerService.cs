using System.Text;
using HackathonContract.Model;
using HackathonContract.Model.Enum;
using HackathonDatabase.model;
using HackathonEveryone.Utils;
using HackathonHrManager.model;
using HackathonRabbitMq;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace HackathonHrManager;

public class HrManagerService : IServiceRunnable
{
    private int countParticipants;
    private readonly List<Employee> jEmployees;
    private readonly List<Employee> tEmployees;

    private readonly Dictionary<string, HackathonContext> Hackathons = new();

    private readonly HrManager _hrManager;
    private static readonly HttpClient Client = new();

    public HrManagerService(
        IConfiguration configuration,
        HrManager hrManager)
    {
        _hrManager = hrManager;
        var juniorFile = configuration["HackathonSettings:JuniorFile"];
        var teamLeadFile = configuration["HackathonSettings:TeamLeadFile"];
        jEmployees = ParseCsv.RunAsync(juniorFile);
        tEmployees = ParseCsv.RunAsync(teamLeadFile);
        countParticipants = jEmployees.Count + tEmployees.Count;
    }

    public void Run(string message)
    {
        var employerResult = JsonSerializer.Deserialize<EmployerResult>(message);
        TryStartHackathon(GetOrInit(employerResult.HackathonId), employerResult);
    }

    private void TryStartHackathon(HackathonContext context, EmployerResult employerResult)
    {
        switch (EnumExtensions.GetEmployeeTypeByDisplayName(employerResult.EmployeeType))
        {
            case EmployeeType.Junior:
                context.jWishlists.Add(employerResult.Wishlist);
                break;
            case EmployeeType.TeamLead:
                context.tWishlists.Add(employerResult.Wishlist);
                break;
        }

        var countCurrentParticipants = context.jWishlists.Count + context.tWishlists.Count;
        Console.WriteLine("Update statistic: " + countCurrentParticipants);

        if (countCurrentParticipants == countParticipants)
        {
            Console.WriteLine("Start organize hackathon");
            var hackathonMetaInfo = _hrManager.OrganizeHackathon(
                jEmployees,
                tEmployees,
                context.tWishlists,
                context.jWishlists
            );

            var requestBody = new HackathonResult(
                jEmployees,
                tEmployees,
                hackathonMetaInfo
            );
            var content = new StringContent(JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            Client.PostAsync("http://hrDirector:8080/api/send_hackathon", content)
                .GetAwaiter().GetResult();
        }
    }

    private HackathonContext GetOrInit(string id)
    {
        if (Hackathons.TryGetValue(id, out var hackathonContext))
        {
            return hackathonContext;
        }

        Hackathons[id] = new HackathonContext([], []);
        return Hackathons[id];
    }
}