using HackathonContract.Model;
using HackathonDatabase.model;

namespace HackathonDatabase.service;

public class HackathonService(ApplicationDbContext applicationDbContext) : IHackathonService
{
    public void SaveHackathon(double harmonicMean, HackathonMetaInfo hackathonMetaInfo)
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
                employeeId = w.EmployeeId,
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
    }
}