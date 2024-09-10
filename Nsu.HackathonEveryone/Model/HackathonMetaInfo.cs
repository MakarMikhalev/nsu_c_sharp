namespace HackathonEveryone.Model;

public record HackathonMetaInfo(
    IEnumerable<Wishlist> TeamLeadsWishlists,
    IEnumerable<Wishlist> JuniorsWishlists,
    IEnumerable<Team> Teams);