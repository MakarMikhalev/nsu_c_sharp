using HackathonContract.Model;

namespace HackathonContract.ServiceContract;

public interface ITeamBuildingStrategy
{
    /// <summary>
    /// Распределяет тимилидов и джунов по командам
    /// </summary>
    /// <param name="teamLeads">Тимлиды</param>
    /// <param name="juniors">Джуны</param>
    /// <returns>Список команд</returns>
    IEnumerable<Team> BuildTeams(
        IEnumerable<Employee> teamLeads,
        IEnumerable<Employee> juniors,
        IEnumerable<Wishlist> teamLeadsWishlists, 
        IEnumerable<Wishlist> juniorsWishlists);
}