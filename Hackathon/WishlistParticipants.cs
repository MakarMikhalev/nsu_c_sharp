using HackathonContract.Model;

namespace HackathonRunner;

public record WishlistParticipants(
    IEnumerable<Wishlist> teamLeadsWishlists,
    IEnumerable<Wishlist> juniorsWishlists);