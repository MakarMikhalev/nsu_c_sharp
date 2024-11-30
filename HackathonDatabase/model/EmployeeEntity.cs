using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HackathonDatabase.model;

[Table("employee", Schema = "public")]
public class EmployeeEntity
{
    [Key]
    [Column(Order = 0)]
    public int Id { get; set; }

    [Key]
    [Column(Order = 1)]
    public EmployeeType EmployeeType { get; set; }

    public string Name { get; set; } = null!;
}