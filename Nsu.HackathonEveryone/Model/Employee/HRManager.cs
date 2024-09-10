using HackathonEveryone.ServiceContract;
using HackathonEveryone.ServiceContract.Impl;

namespace HackathonEveryone.Model.Employee;

public class HrManager
{
    private readonly ITeamBuildingStrategy _teamBuildingStrategy = new TeamBuildingStrategy();
    private readonly IWishlistGenerator _wishlistGenerator = new WishlistGenerator();

    public HackathonMetaInfo OrganizeHackathon(IEnumerable<Employee> juniors,
        IEnumerable<Employee> teamLeads)
    {
        var requestingEmployees = juniors as Employee[] ?? juniors.ToArray();
        var availableEmployees = teamLeads as Employee[] ?? teamLeads.ToArray();

        var wishlistsJuniors =
            _wishlistGenerator.GenerateWishlists(requestingEmployees, availableEmployees);

        var wishlistTeamLeads =
            _wishlistGenerator.GenerateWishlists(availableEmployees, requestingEmployees);

        var teamLeadsWishlists = wishlistTeamLeads as Wishlist[] ?? wishlistTeamLeads.ToArray();

        var juniorsWishlists = wishlistsJuniors as Wishlist[] ?? wishlistsJuniors.ToArray();

        var teams = _teamBuildingStrategy.BuildTeams(
            requestingEmployees,
            availableEmployees,
            juniorsWishlists,
            teamLeadsWishlists);

        return new HackathonMetaInfo(teamLeadsWishlists, juniorsWishlists, teams);
    }
}