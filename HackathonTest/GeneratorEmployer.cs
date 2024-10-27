using HackathonContract.Model;
using HackathonDatabase.model;

namespace HackathonTest;

public static class GeneratorEmployer
{
    public static IEnumerable<Employee> GenerateJuniors()
    {
        return new List<Employee>
        {
            new(1, "Junior-1"),
            new(2, "Junior-2"),
            new(3, "Junior-3")
        };
    }
    
    public static IEnumerable<EmployeeEntity> GenerateJuniorEntitys()
    {
        return new List<EmployeeEntity>
        {
            new()
            {
                Id = 1,
                Name = "Junior-1"   
            },
            new()
            {
                Id = 2,
                Name = "Junior-2"   
            },
            new()
            {
                Id = 3,
                Name = "Junior-3"   
            }
        };
    }

    public static IEnumerable<Employee> GenerateSeniors()
    {
        return new List<Employee>
        {
            new(1, "Senior-1"),
            new(2, "Senior-2"),
            new(3, "Senior-3")
        };
    }
    public static IEnumerable<EmployeeEntity> GenerateSeniorEntitys()
    {
        return new List<EmployeeEntity>
        {
            new()
            {
                Id = 1,
                Name = "Senior-1"   
            },
            new()
            {
                Id = 2,
                Name = "Senior-2"   
            },
            new()
            {
                Id = 3,
                Name = "Senior-3"   
            }
        };
    }
}