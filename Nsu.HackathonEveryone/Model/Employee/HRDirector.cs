namespace HackathonEveryone.Model.Employee;

public sealed class HrDirector
{
    public double CalculateMeanHarmonic(HackathonMetaInfo hackathonMetaInfo)
    {
        var totalSatisfactionScores = new List<double>();

        var teamLeadsWishlistsDict = hackathonMetaInfo.TeamLeadsWishlists
            .ToDictionary(w => w.EmployeeId, w => w);

        var juniorsWishlistsDict = hackathonMetaInfo.JuniorsWishlists
            .ToDictionary(w => w.EmployeeId, w => w);
        foreach (var team in hackathonMetaInfo.Teams)
        {
            if (teamLeadsWishlistsDict.TryGetValue(team.TeamLead.Id, out var teamLeadWishlist))
                totalSatisfactionScores.Add(CalculateSatisfactionScore(team.Junior.Id,
                    teamLeadWishlist.DesiredEmployees));

            if (juniorsWishlistsDict.TryGetValue(team.Junior.Id, out var juniorWishlist))
                totalSatisfactionScores.Add(CalculateSatisfactionScore(team.TeamLead.Id,
                    juniorWishlist.DesiredEmployees));
        }

        return CalculateHarmonic(totalSatisfactionScores);
    }

    private double CalculateSatisfactionScore(int desiredEmployeeId, int[] wishlistIds)
    {
        var rank = Array.IndexOf(wishlistIds, desiredEmployeeId);
        return 20 - rank;
    }

    private double CalculateHarmonic(IEnumerable<double> values)
    {
        var sumOfInverses = values.Sum(v => 1 / v);
        return values.Count() / sumOfInverses;
    }
}