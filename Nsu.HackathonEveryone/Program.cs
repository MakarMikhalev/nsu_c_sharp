using Microsoft.Extensions.Configuration;

namespace HackathonEveryone;

public static class Program
{ 
    public static void Main()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        
        var app = new AppHackathon();
        app.Run(builder.Build());
    }
}