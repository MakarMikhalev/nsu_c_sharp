using System.Text;
using HackathonContract.Model;
using HackathonContract.Model.Enum;
using HackathonDatabase.model;
using HackathonEveryone.Utils;
using Microsoft.Extensions.Configuration;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace HackthonEmployee;

public class Program
{
    private static readonly HttpClient Client = new();

    public static async Task Main()
    {
        Console.WriteLine("Start task create employee");

        Console.WriteLine(Environment.GetEnvironmentVariable("EMPLOYER_TYPE"));
        Console.WriteLine(Environment.GetEnvironmentVariable("EMPLOYER_ID"));

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        var type = Environment.GetEnvironmentVariable("EMPLOYER_TYPE");
        var id = int.Parse(Environment.GetEnvironmentVariable("EMPLOYER_ID"));

        Console.WriteLine("Employee id: " + id + "  type: " + type);

        var juniorFile = configuration["HackathonSettings:JuniorFile"];
        var teamLeadFile = configuration["HackathonSettings:TeamLeadFile"];

        var content = new StringContent(JsonSerializer.Serialize(
                CreateWishlist(id, type, juniorFile, teamLeadFile)),
            Encoding.UTF8,
            "application/json"
        );

        await SendRequest(type, content);
    }

    private static async Task SendRequest(string type, StringContent content)
    {
        var url = $"http://hrManager:8081/api/send_wishlist?type=" + type;
        Console.WriteLine("Url " + url);
        var response = await Client.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseBody);
    }

    private static Wishlist? CreateWishlist(
        int id,
        string type,
        string juniorFile,
        string teamLeadFile)
    {
        Console.WriteLine("Create wish list");
        var employeeType = EnumExtensions.GetEmployeeTypeByDisplayName(type);
        return employeeType switch
        {
            EmployeeType.Junior => ParseCsv.RunAsync(juniorFile).Find(e => e.Id == id)
                .CreateWishlist(ParseCsv.RunAsync(juniorFile)),
            EmployeeType.TeamLead => ParseCsv.RunAsync(teamLeadFile).Find(e => e.Id == id)
                .CreateWishlist(ParseCsv.RunAsync(teamLeadFile)),
            _ => null
        };
    }
}