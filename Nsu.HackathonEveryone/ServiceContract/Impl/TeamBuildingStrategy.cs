using HackathonEveryone.Model;
using HackathonEveryone.Model.Employee;

namespace HackathonEveryone.ServiceContract.Impl;

public sealed class TeamBuildingStrategy : ITeamBuildingStrategy
{
    public IEnumerable<Team> BuildTeams(IEnumerable<Employee> teamLeads,
        IEnumerable<Employee> juniors, IEnumerable<Wishlist> teamLeadsWishlists,
        IEnumerable<Wishlist> juniorsWishlists)
    {
        var teams = new List<Team>();
        var availableJuniors = new HashSet<Employee?>(juniors);
        var availableTeamLeads = new HashSet<Employee?>(teamLeads);

        var teamLeadsWishlistsDict = teamLeadsWishlists.ToDictionary(w => w.EmployeeId, w => w);
        var juniorsWishlistsDict = juniorsWishlists.ToDictionary(w => w.EmployeeId, w => w);
        while (availableJuniors.Any() && availableTeamLeads.Any())
        {
            Employee? bestJunior = null;
            Employee? bestTeamLead = null;
            var maxSatisfactionScore = int.MinValue;

            foreach (var junior in availableJuniors)
            {
                if (junior == null) continue;
                var juniorWishlist = juniorsWishlistsDict[junior.Id];

                foreach (var teamLead in availableTeamLeads)
                {
                    if (teamLead == null) continue;
                    var teamLeadWishlist = teamLeadsWishlistsDict[teamLead.Id];
                    var satisfactionScore = CalculateSatisfactionScore(junior, teamLead,
                        juniorWishlist, teamLeadWishlist);

                    if (satisfactionScore <= maxSatisfactionScore) continue;
                    maxSatisfactionScore = satisfactionScore;
                    bestJunior = junior;
                    bestTeamLead = teamLead;
                }
            }

            if (bestJunior == null) continue;

            teams.Add(new Team(bestJunior, bestTeamLead));
            availableJuniors.Remove(bestJunior);
            availableTeamLeads.Remove(bestTeamLead);
        }

        return teams;
    }

    private static int CalculateSatisfactionScore(Employee junior, Employee teamLead,
        Wishlist juniorWishlist, Wishlist teamLeadWishlist)
    {
        return GetPreferenceIndex(juniorWishlist.DesiredEmployees, teamLead.Id) +
               GetPreferenceIndex(teamLeadWishlist.DesiredEmployees, junior.Id);
    }

    private static int GetPreferenceIndex(int[] desiredEmployees, int employeeId)
    {
        var index = Array.IndexOf(desiredEmployees, employeeId);
        return index >= 0 ? index + 1 : 0;
    }
}