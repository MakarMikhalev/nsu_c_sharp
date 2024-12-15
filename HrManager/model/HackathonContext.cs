using HackathonContract.Model;

namespace HackathonHrManager.model;

public record HackathonContext(
    List<Wishlist> jWishlists,
    List<Wishlist> tWishlists);