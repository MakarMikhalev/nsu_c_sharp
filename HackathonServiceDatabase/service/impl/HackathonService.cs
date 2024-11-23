using HackathonContract.Model;
using HackathonDatabase.model;
using Microsoft.EntityFrameworkCore;

namespace HackathonDatabase.service;

public class HackathonService(ApplicationDbContext applicationDbContext) : IHackathonService
{
    public int SaveHackathon(double harmonicMean, HackathonMetaInfo hackathonMetaInfo)
    {
        var wishlists =
            hackathonMetaInfo.JuniorsWishlists.Concat(hackathonMetaInfo.TeamLeadsWishlists);

        var hackathon = new HackathonEntity
        {
            HarmonicMean = harmonicMean,
            Teams = hackathonMetaInfo.Teams.Select(t => new TeamEntity
            {
                TeamLeadId = t.TeamLead.Id,
                JuniorId = t.Junior.Id
            }).ToList(),
            Wishlists = wishlists.Select(w => new WishlistEntity
            {
                EmployeeId = w.EmployeeId,
                DesiredEmployeeIds = w.DesiredEmployees.ToList()
            }).ToList()
        };

        applicationDbContext.HackathonEntities.Add(hackathon);
        try
        {
            applicationDbContext.SaveChanges();
        }
        catch
            (Exception ex)
        {
            Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
        }
        return hackathon.Id;
    }

    public HackathonEntity GetHackathonById(int id)
    {
        Console.WriteLine("");
        return applicationDbContext.HackathonEntities
            .Include(h => h.Teams)
            .Include(h => h.Wishlists)
            .FirstOrDefault(h => h.Id == id);
    }
    
    public double CalculateAverageHarmonicMean()
    {
        return applicationDbContext.HackathonEntities
            .Select(h => h.HarmonicMean)
            .ToList().Average();
    }
}