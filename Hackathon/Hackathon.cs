using HackathonContract.Model;

namespace HackathonRunner;

public class Hackathon
{
    public virtual WishlistParticipants Start(IEnumerable<Employee> jEnumerable,
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
        
        return requestingEmployees
            .Select(e => new Wishlist(
                e.Id,
                availableEmployeesIds
                    .OrderBy(_ => Guid.NewGuid())
                    .ToArray()
            ))
            .ToList();
    }
}