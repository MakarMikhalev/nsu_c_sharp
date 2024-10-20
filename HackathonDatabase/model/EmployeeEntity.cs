using System.ComponentModel.DataAnnotations.Schema;

namespace HackathonDatabase.model;

[Table("employee", Schema = "public")]
public class EmployeeEntity
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    public EmployeeType EmployeeType { get; set; }
}