using HackathonContract.Model;
using HackathonDatabase.model;

namespace HackathonRunner;

public class Hackathon
{
    public virtual WishlistParticipants Start(IEnumerable<EmployeeEntity> jEnumerable,
        IEnumerable<EmployeeEntity> tEnumerable)
    {
        return new WishlistParticipants(
            CreateWishlist(tEnumerable, jEnumerable),
            CreateWishlist(jEnumerable, tEnumerable));
    }

    private IEnumerable<Wishlist> CreateWishlist(IEnumerable<EmployeeEntity> requestingEmployees,
        IEnumerable<EmployeeEntity> availableEmployees)
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