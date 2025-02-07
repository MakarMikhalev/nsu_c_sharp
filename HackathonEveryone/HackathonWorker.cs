﻿using HackathonEveryone.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace HackathonEveryone;

public class HackathonWorker : IHostedService
{
    private readonly HackathonRunner _hackathonRunner;
    private readonly IConfiguration _configuration;

    public HackathonWorker(
        HackathonRunner hackathonRunner,
        IConfiguration configuration)
    {
        _hackathonRunner = hackathonRunner;
        _configuration = configuration;
    }

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
            var juniorFile = _configuration["HackathonSettings:JuniorFile"];
            var teamLeadFile = _configuration["HackathonSettings:TeamLeadFile"];
            var countIteration =
                int.Parse(_configuration["HackathonSettings:CountIteration"] ?? string.Empty);
            
            var juniors = ParseCsv.RunAsync(juniorFile);
            var teamLeads = ParseCsv.RunAsync(teamLeadFile);
            for (var i = 1; i <= countIteration; ++i)
            {
                _hackathonRunner.Run(juniors, teamLeads);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Run: message exception: {ex.Message}");
        }
    }
}