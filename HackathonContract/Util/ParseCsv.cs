using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using HackathonContract.Model;

namespace HackathonEveryone.Utils;

public static class ParseCsv
{
    public static List<Employee> RunAsync(string filePath)
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

    public static List<string> RunAsyncNamesQueue(params string[] filePaths)
    {
        var allNames = new List<string>();

        foreach (var filePath in filePaths)
        {
            var employees = RunAsync(filePath);
            var names = employees
                .Select(emp => $"{emp.GetType()}_{emp.Id}")
                .ToList();

            allNames.AddRange(names);
        }

        return allNames;
    }
}