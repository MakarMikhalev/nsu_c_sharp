using HackathonRunner;
using HackathonContract.Model;
using HackathonContract.ServiceContract;

namespace HackathonHrManager;

public class HrManager(ITeamBuildingStrategy teamBuildingStrategy)
{
    public HackathonMetaInfo OrganizeHackathon(
        IEnumerable<Employee?> juniors,
        IEnumerable<Employee?> teamLeads,
        WishlistParticipants wishlistParticipants)
    {
        return OrganizeHackathon(
            juniors,
            teamLeads,
            wishlistParticipants.JuniorsWishlists,
            wishlistParticipants.TeamLeadsWishlists);
    }

    public HackathonMetaInfo OrganizeHackathon(
        IEnumerable<Employee?> juniors,
        IEnumerable<Employee?> teamLeads,
        IEnumerable<Wishlist> teamLeadsWishlists,
        IEnumerable<Wishlist> juniorsWishlists)
    {
        var requestingEmployees = juniors.ToArray();
        var availableEmployees = teamLeads.ToArray();
        var teams = teamBuildingStrategy.BuildTeams(
            requestingEmployees,
            availableEmployees,
            juniorsWishlists,
            teamLeadsWishlists);

        return new HackathonMetaInfo(
            teamLeadsWishlists,
            juniorsWishlists,
            teams);
    }
}