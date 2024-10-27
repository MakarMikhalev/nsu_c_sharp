using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HackathonDatabase.model;

[Table("wishlist", Schema = "public")]
public class WishlistEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int employeeId { get; set; }
    
    public List<int> DesiredEmployeeIds { get; set; } = new();
    
    public int HackathonId { get; set; }
    
    public HackathonEntity Hackathon { get; set; }
}