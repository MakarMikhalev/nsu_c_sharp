using HackathonContract.Model;

namespace HackathonHrManager;

public record HackathonMetaInfo(
    IEnumerable<Wishlist> TeamLeadsWishlists,
    IEnumerable<Wishlist> JuniorsWishlists,
    IEnumerable<Team> Teams
    );
    