using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HackathonDatabase.model;

[Table("wishlist", Schema = "public")]
public class WishlistEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int EmployeeId { get; set; }
    
    public List<int> DesiredEmployeeIds { get; set; } = [];
    
    [ForeignKey(nameof(Hackathon))]
    public int HackathonId { get; set; }
    
    public HackathonEntity Hackathon { get; set; } = null!;
}