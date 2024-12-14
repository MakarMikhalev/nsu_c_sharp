using System.Text;
using HackathonContract.Model;
using HackathonContract.Model.Enum;
using HackathonDatabase.model;
using HackathonEveryone.Utils;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace HackathonHrManager;

public class HrManagerService
{
    private int countParticipants;
    private List<Wishlist> jWishlists = new();
    private List<Wishlist> tWishlists = new();

    private readonly List<Employee> jEmployees;
    private readonly List<Employee> tEmployees;
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

    public void TryStartHackathon(string type, Wishlist wishlist)
    {
        Console.WriteLine("Try start hackathon, type: " + type);
        switch (EnumExtensions.GetEmployeeTypeByDisplayName(type))
        {
            case EmployeeType.Junior:
                jWishlists.Add(wishlist);
                break;
            case EmployeeType.TeamLead:
                tWishlists.Add(wishlist);
                break;
        }

        Console.WriteLine("junior count " + jWishlists.Count);
        Console.WriteLine("team lead count " + tWishlists.Count);
        var countCurrentParticipants = jWishlists.Count + tWishlists.Count;
        Console.WriteLine("Update statistic: " + countCurrentParticipants);

        if (countCurrentParticipants == countParticipants)
        {
            Console.WriteLine("Start organize hackathon");
            var hackathonMetaInfo = _hrManager.OrganizeHackathon(
                jEmployees,
                tEmployees,
                tWishlists,
                jWishlists
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
            var response = Client.PostAsync("http://hrDirector:8080/api/send_hackathon", content).GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Request sent successfully.");
            }
            else
            {
                Console.WriteLine($"Error sending request: {response.StatusCode}");
            }
        }
    }
}