using HackathonContract.Model;
using HackathonContract.ServiceContract;

namespace HackathonStrategy;

public class TeamBuildingStrategy : ITeamBuildingStrategy
{
    public IEnumerable<Team> BuildTeams(
        IEnumerable<Employee?> teamLeads,
        IEnumerable<Employee?> juniors,
        IEnumerable<Wishlist> teamLeadWishlists,
        IEnumerable<Wishlist> juniorWishlists)
    {
        var unmatchedJuniors = GetUnmatchedJuniors(juniorWishlists);
        var assignments = new Dictionary<int, int>();
        var juniorPreferences = GetJuniorPreferences(juniorWishlists);

        MatchJuniorsToLeads(unmatchedJuniors, assignments, juniorPreferences, teamLeadWishlists);

        return CreateTeams(teamLeads, juniors, assignments);
    }

    private HashSet<int> GetUnmatchedJuniors(IEnumerable<Wishlist> juniorWishlists)
    {
        return new HashSet<int>(juniorWishlists.Select(wishlist => wishlist.EmployeeId));
    }

    private Dictionary<int, Queue<int>> GetJuniorPreferences(IEnumerable<Wishlist> juniorWishlists)
    {
        return juniorWishlists.ToDictionary(
            wishlist => wishlist.EmployeeId,
            wishlist => new Queue<int>(wishlist.DesiredEmployees));
    }

    private void MatchJuniorsToLeads(
        HashSet<int> unmatchedJuniors,
        Dictionary<int, int> assignments,
        Dictionary<int, Queue<int>> juniorPreferences,
        IEnumerable<Wishlist> teamLeadWishlists)
    {
        while (unmatchedJuniors.Any())
        {
            var juniorId = unmatchedJuniors.First();
            unmatchedJuniors.Remove(juniorId);

            if (!juniorPreferences[juniorId].Any())
                continue;

            var preferredLeadId = juniorPreferences[juniorId].Dequeue();
            ProcessMatch(juniorId, preferredLeadId, unmatchedJuniors, assignments,
                teamLeadWishlists);
        }
    }

    private void ProcessMatch(
        int juniorId,
        int preferredLeadId,
        HashSet<int> unmatchedJuniors,
        Dictionary<int, int> assignments,
        IEnumerable<Wishlist> teamLeadWishlists)
    {
        if (!assignments.ContainsKey(preferredLeadId))
        {
            assignments[preferredLeadId] = juniorId;
        }
        else
        {
            var existingJuniorId = assignments[preferredLeadId];
            if (IsPreferredJunior(teamLeadWishlists, preferredLeadId, juniorId, existingJuniorId))
            {
                unmatchedJuniors.Add(existingJuniorId);
                assignments[preferredLeadId] = juniorId;
            }
            else
            {
                unmatchedJuniors.Add(juniorId);
            }
        }
    }

    private bool IsPreferredJunior(
        IEnumerable<Wishlist> teamLeadWishlists,
        int leadId,
        int newJuniorId,
        int currentJuniorId)
    {
        var leadWishlist = teamLeadWishlists.First(wishlist => wishlist.EmployeeId == leadId)
            .DesiredEmployees;
        return Array.IndexOf(leadWishlist, newJuniorId) <
               Array.IndexOf(leadWishlist, currentJuniorId);
    }

    private IEnumerable<Team> CreateTeams(
        IEnumerable<Employee?> teamLeads,
        IEnumerable<Employee?> juniors,
        Dictionary<int, int> assignments)
    {
        var leadDictionary = teamLeads.ToDictionary(lead => lead.Id);
        var juniorDictionary = juniors.ToDictionary(junior => junior.Id);

        return assignments.Select(pair => new Team(
            leadDictionary[pair.Key],
            juniorDictionary[pair.Value]));
    }
}