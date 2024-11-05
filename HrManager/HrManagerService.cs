using System.Text;
using HackathonContract.Model;
using HackathonContract.Model.Enum;
using HackathonDatabase.model;
using HackathonEveryone.Utils;
using Microsoft.Testing.Platform.Configurations;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace HackathonHrManager;

public class HrManagerService
{
    private int countParticipants;
    private int countCurrentParticipants;
    private readonly object _lockObj = new();
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

    public void tryStartHackathon(string type, Wishlist wishlist)
    {
        lock (_lockObj)
        {
            switch (EnumExtensions.GetEmployeeTypeByDisplayName(type))
            {
                case EmployeeType.Junior:
                    jWishlists.Add(wishlist);
                    break;
                case EmployeeType.TeamLead:
                    tWishlists.Add(wishlist);
                    break;
            }

            ++countCurrentParticipants;

            if (countParticipants == countCurrentParticipants)
            {
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
                Client.PostAsync($"http://localhost:/api/send_hackathon", content);
            }
        }
    }
}