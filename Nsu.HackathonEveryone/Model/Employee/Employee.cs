namespace HackathonEveryone.Model.Employee;

public record Employee(int Id, string Name)
{
    public Wishlist getWishlist(IEnumerable<Employee> availableEmployees)
    {
        return new Wishlist(Id, availableEmployees
            .Distinct()
            .Select(e => e.Id)
            .OrderBy(_ => Guid.NewGuid())
            .ToArray());
    }
}