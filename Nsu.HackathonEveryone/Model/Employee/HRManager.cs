using HackathonEveryone.ServiceContract;

namespace HackathonEveryone.Model.Employee;

public class HrManager
{
    private readonly ITeamBuildingStrategy _teamBuildingStrategy;

    public HrManager(ITeamBuildingStrategy _teamBuildingStrategy)
    {
        this._teamBuildingStrategy = _teamBuildingStrategy;
    }

    public HackathonMetaInfo OrganizeHackathon(IEnumerable<Employee> juniors,
        IEnumerable<Employee> teamLeads)
    {
        var requestingEmployees = juniors.ToArray();
        var availableEmployees = teamLeads.ToArray();

        var teamLeadsWishlists = juniors.Distinct()
            .Select(e => e.getWishlist(juniors))
            .ToList();

        var juniorsWishlists = teamLeads.Distinct()
            .Select(e => e.getWishlist(teamLeads))
            .ToList();

        var teams = _teamBuildingStrategy.BuildTeams(
            requestingEmployees,
            availableEmployees,
            juniorsWishlists,
            teamLeadsWishlists);

        return new HackathonMetaInfo(teamLeadsWishlists, juniorsWishlists, teams);
    }
}