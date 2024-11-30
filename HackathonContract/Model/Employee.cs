namespace HackathonContract.Model;

public record Employee(
    int Id,
    string Name)
{
    public Wishlist CreateWishlist(IEnumerable<Employee> requestingEmployees)
    {
        return new Wishlist(
            Id,
            requestingEmployees
                .OrderBy(_ => Guid.NewGuid())
                .Select(e => e.Id)
                .ToArray());
    }
}