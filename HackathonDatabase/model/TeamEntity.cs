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
    
    public EmployeeType TeamLeadEmployeeType { get; set; }
    
    public EmployeeEntity TeamLead { get; }

    public int JuniorId { get; set; }
    
    public EmployeeType JuniorEmployeeType { get; set; }
    
    public EmployeeEntity Junior { get; }

    [ForeignKey(nameof(Hackathon))] public int HackathonId { get; set; }

    public HackathonEntity Hackathon { get; set; } = null!;
}