using HackathonEveryone.Model;
using HackathonEveryone.Model.Employee;

namespace HackathonEveryone.ServiceContract.Impl;

public class WishlistGenerator : IWishlistGenerator
{
    public IEnumerable<Wishlist> GenerateWishlists(IEnumerable<Employee> requestingEmployees,
        IEnumerable<Employee> availableEmployees)
    {
        var availableEmployeesIds = availableEmployees.Distinct().Select(e => e.Id).ToList();

        return (from requestingEmployee in requestingEmployees
            let deserializeAvailableEmployees =
                availableEmployeesIds.OrderBy(_ => Guid.NewGuid()).ToArray()
            select new Wishlist(requestingEmployee.Id, deserializeAvailableEmployees)).ToList();
    }
}