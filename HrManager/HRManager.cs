using HackathonRunner;
using HackathonContract.Model;
using HackathonContract.ServiceContract;

namespace HackathonHrManager;

public class HrManager(ITeamBuildingStrategy teamBuildingStrategy)
{
    public HackathonMetaInfo OrganizeHackathon(
        IEnumerable<Employee> juniors,
        IEnumerable<Employee> teamLeads,
        WishlistParticipants wishlistParticipants)
    {
        var requestingEmployees = juniors.ToArray();
        var availableEmployees = teamLeads.ToArray();
        var teams = teamBuildingStrategy.BuildTeams(
            requestingEmployees,
            availableEmployees,
            wishlistParticipants.JuniorsWishlists,
            wishlistParticipants.TeamLeadsWishlists);

        return new HackathonMetaInfo(
            wishlistParticipants.JuniorsWishlists,
            wishlistParticipants.TeamLeadsWishlists,
            teams);
    }
}
