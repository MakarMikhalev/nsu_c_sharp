using Nsu.HackathonContract.Model;

namespace Nsu.Hackathon;

public record WishlistParticipants(
    IEnumerable<Wishlist> teamLeadsWishlists,
    IEnumerable<Wishlist> juniorsWishlists);