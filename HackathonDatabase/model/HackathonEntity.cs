using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HackathonDatabase.model;

[Table("hackathon", Schema = "public")]
public class HackathonEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public double HarmonicMean { get; set; }

    public List<TeamEntity> Teams { get; set; } = [];

    public List<WishlistEntity> Wishlists { get; set; } = [];
}