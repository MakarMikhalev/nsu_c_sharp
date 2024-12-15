namespace HackthonEmployee;

using System.Text;
using HackathonContract.Model;
using HackathonContract.Model.Enum;
using HackathonDatabase.model;
using HackathonEveryone.Utils;
using HackathonRabbitMq;
using Microsoft.Extensions.Configuration;
using JsonSerializer = System.Text.Json.JsonSerializer;

public class EmployerService(IRabbitMqService rabbitMqService) : IServiceRunnable
{
    public void Run(string hackathonId)
    {
        Console.WriteLine("Start task create employee");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        var type = Environment.GetEnvironmentVariable("EMPLOYER_TYPE");
        var id = int.Parse(Environment.GetEnvironmentVariable("EMPLOYER_ID"));

        var juniorFile = configuration["HackathonSettings:JuniorFile"];
        var teamLeadFile = configuration["HackathonSettings:TeamLeadFile"];
        
        var wishlist = CreateWishlist(id, type, juniorFile, teamLeadFile);
        
        var content = new StringContent(JsonSerializer.Serialize(
            new EmployerResult(hackathonId, type, wishlist)),
            Encoding.UTF8,
            "application/json"
        );

        rabbitMqService.SendMessage(content.ToString());
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