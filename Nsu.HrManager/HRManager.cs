using Nsu.Hackathon;
using Nsu.HackathonContract.ServiceContract;

namespace HackathonHrManager;

public class HrManager(ITeamBuildingStrategy teamBuildingStrategy)
{
    public HackathonMetaInfo OrganizeHackathon(
        IEnumerable<Nsu.HackathonContract.Model.Employee> juniors,
        IEnumerable<Nsu.HackathonContract.Model.Employee> teamLeads,
        WishlistParticipants wishlistParticipants)
    {
        var requestingEmployees = juniors.ToArray();
        var availableEmployees = teamLeads.ToArray();

        var teams = teamBuildingStrategy.BuildTeams(
            requestingEmployees,
            availableEmployees,
            wishlistParticipants.juniorsWishlists,
            wishlistParticipants.teamLeadsWishlists);

        return new HackathonMetaInfo(
            wishlistParticipants.juniorsWishlists,
            wishlistParticipants.teamLeadsWishlists,
            teams);
    }
}