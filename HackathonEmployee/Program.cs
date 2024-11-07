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
    private const int INDEX_VALUE = 1;

    private static readonly HttpClient Client = new();

    public static async Task Main(string[] args)
    {
        try
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var type = Array.Find(args, a => a.StartsWith("type="))?.Split('=')[INDEX_VALUE];
            var id = int.Parse(
                Array.Find(args, a => a.StartsWith("id="))?.Split('=')[INDEX_VALUE] ??
                string.Empty);

            var juniorFile = configuration["HackathonSettings:JuniorFile"];
            var teamLeadFile = configuration["HackathonSettings:TeamLeadFile"];

            var content = new StringContent(JsonSerializer.Serialize(
                    CreateWishlist(id, type, juniorFile, teamLeadFile)),
                Encoding.UTF8,
                "application/json"
            );
            
            var url = $"http://localhost:/api/send_wishlist?type={type}";
            var response = await Client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Ошибка запроса: {e.Message}");
        }
    }

    private static Wishlist? CreateWishlist(
        int id,
        string type,
        string juniorFile,
        string teamLeadFile)
    {
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