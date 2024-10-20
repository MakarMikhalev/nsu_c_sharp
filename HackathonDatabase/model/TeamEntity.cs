using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HackathonDatabase.model;

[Table("team", Schema = "public")]
public class TeamEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int TeamLeadId { get; set; }

    public int JuniorId { get; set; }
    
    public int HackathonId { get; set; }
    
    public HackathonEntity Hackathon { get; set; }
}