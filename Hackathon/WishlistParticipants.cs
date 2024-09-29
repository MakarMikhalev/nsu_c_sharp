using HackathonContract.Model;

namespace HackathonRunner;

public record WishlistParticipants(
    IEnumerable<Wishlist> TeamLeadsWishlists,
    IEnumerable<Wishlist> JuniorsWishlists);