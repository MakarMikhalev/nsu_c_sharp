using HackathonContract.Model;

namespace HackathonRunner;

public class Hackathon
{
    public WishlistParticipants Start(IEnumerable<Employee> jEnumerable,
        IEnumerable<Employee> tEnumerable)
    {
        return new WishlistParticipants(
            CreateWishlist(tEnumerable, jEnumerable),
            CreateWishlist(jEnumerable, tEnumerable));
    }

    private IEnumerable<Wishlist> CreateWishlist(IEnumerable<Employee> requestingEmployees,
        IEnumerable<Employee> availableEmployees)
    {
        var availableEmployeesIds = availableEmployees
            .Distinct()
            .Select(e => e.Id)
            .ToList();

        return (from requestingEmployee in requestingEmployees
            let deserializeAvailableEmployees = availableEmployeesIds
                .OrderBy(_ => Guid.NewGuid())
                .ToArray()
            select new Wishlist(
                requestingEmployee.Id,
                deserializeAvailableEmployees)).ToList();
    }
}