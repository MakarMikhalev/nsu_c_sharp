using HackathonContract.Model;

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

    public static IEnumerable<Employee> GenerateSeniors()
    {
        return new List<Employee>
        {
            new(1, "Senior-1"),
            new(2, "Senior-2"),
            new(3, "Senior-3")
        };
    }
}