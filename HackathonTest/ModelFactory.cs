using HackathonContract.Model;
using HackathonDatabase.model;
using HackathonRunner;

namespace HackathonTest;

public static class ModelFactory
{
    public static IEnumerable<Employee> GenerateEmployees(
        int count,
        string prefix = "Employee"
    )
    {
        var employees = new List<Employee>();
        for (int i = 1; i <= count; ++i)
        {
            employees.Add(new Employee(i, $"{prefix}-{i}"));
        }

        return employees;
    }

    public static IEnumerable<EmployeeEntity> GenerateEmployeeEntities(
        int count,
        string prefix = "Employee"
    ) {
        var employees = new List<EmployeeEntity>();
        for (int i = 1; i <= count; ++i)
        {
            employees.Add(new EmployeeEntity
            {
                Id = i,
                Name = $"{prefix}-{i}"
            });
        }
        return employees;
    }
    
    public static IEnumerable<Team> GenerateTeams(int teamCount)
    {
        var teams = new List<Team>();
        for (int i = 1; i <= teamCount; ++i)
        {
            var teamLead = new Employee(i, $"TeamLead-{i}");
            var junior = new Employee(i, $"Junior-{i}");

            teams.Add(new Team(teamLead, junior));
        }
        return teams;
    }
    
    public static WishlistParticipants GenerateWishlistParticipants(int n)
    {
        var juniors = new List<Wishlist>();
        var teamLeads = new List<Wishlist>();
        var employees = GenerateDesiredEmployees(n);
        for (int i = 1; i <= n; ++i)
        {
            juniors.Add(new Wishlist(i, employees));
            teamLeads.Add(new Wishlist(i, employees));
        }

        return new WishlistParticipants(juniors, teamLeads);
    }
    
    public static int[] GenerateDesiredEmployees(int n)
    {
        return Enumerable.Range(1, n).ToArray();
    }

    public static readonly List<Wishlist> Wishlists =
        new() {
        new(1, new[] { 1, 2, 3 }),
        new(2, new[] { 3, 2, 1 }),
        new(3, new[] { 2, 3, 1 })
    };
}