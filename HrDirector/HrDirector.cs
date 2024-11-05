
using HackathonContract.Model;

namespace HackathonHrDirector;

public class HrDirector
{
    public double CalculateMeanHarmonic(HackathonMetaInfo hackathonMetaInfo)
    {
        ValidateParticipants(hackathonMetaInfo);
        var totalSatisfactionScores = new List<double>();

        var teamLeadsWishlistsDict = hackathonMetaInfo.TeamLeadsWishlists
            .ToDictionary(w => w.EmployeeId, w => w);

        var juniorsWishlistsDict = hackathonMetaInfo.JuniorsWishlists
            .ToDictionary(w => w.EmployeeId, w => w);
        foreach (var team in hackathonMetaInfo.Teams)
        {
            if (teamLeadsWishlistsDict.TryGetValue(team.TeamLead.Id, out var teamLeadWishlist))
            {
                totalSatisfactionScores.Add(
                    CalculateSatisfactionScore(
                        team.Junior.Id,
                        teamLeadWishlist.DesiredEmployees
                    )
                );
            }

            if (juniorsWishlistsDict.TryGetValue(team.Junior.Id, out var juniorWishlist))
            {
                totalSatisfactionScores.Add(
                    CalculateSatisfactionScore(
                        team.TeamLead.Id,
                        juniorWishlist.DesiredEmployees)
                );
            }
        }

        return CalculateHarmonic(totalSatisfactionScores);
    }

    public double CalculateSatisfactionScore(int desiredEmployeeId, int[] wishlistIds)
    {
        var rank = Array.IndexOf(wishlistIds, desiredEmployeeId);
        var countParticipants = wishlistIds.Length;
        return countParticipants - rank;
    }

    public double CalculateHarmonic(IEnumerable<double> values)
    {
        var sumOfInverses = values.Sum(v => 1 / v);
        return values.Count() / sumOfInverses;
    }

    private void ValidateParticipants(HackathonMetaInfo hackathonMetaInfo)
    {
        if (hackathonMetaInfo.TeamLeadsWishlists.Any(w => w == null) ||
            hackathonMetaInfo.JuniorsWishlists.Any(w => w == null))
        {
            throw new InvalidOperationException(
                "Список участников содержит некорректные данные: один или несколько элементов равны null.");
        }

        if (hackathonMetaInfo.TeamLeadsWishlists.Count() !=
            hackathonMetaInfo.JuniorsWishlists.Count())
        {
            throw new InvalidOperationException(
                "Невозможно сформировать команды: количество тимлидов и количество участников не совпадает.");
        }
    }
}