using HackathonContract.Model;
using HackathonDatabase.mapper;
using HackathonDatabase.model;
using Microsoft.EntityFrameworkCore;

namespace HackathonDatabase.service;

public class HackathonService
{
    private readonly ApplicationDbContext _applicationDbContext;

    public HackathonService(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

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

        _applicationDbContext.HackathonEntities.Add(hackathon);
        try
        {
            _applicationDbContext.SaveChanges();
        }
        catch
            (Exception ex)
        {
            Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
        }
    }
}