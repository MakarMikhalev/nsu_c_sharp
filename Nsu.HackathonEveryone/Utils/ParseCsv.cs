using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using HackathonEveryone.Model.Employee;

namespace HackathonEveryone.Utils;

public abstract class ParseCsv
{
    public static List<Employee> RunAsync(string? filePath)
    {
        var employees = new List<Employee>();

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";"
        });
        var records = csv.GetRecords<Employee>();
        employees.AddRange(records);
        
        return employees;
    }
}