using System.ComponentModel;
using System.Reflection;
using HackathonDatabase.model;

namespace HackathonContract.Model.Enum;

public static class EnumExtensions
{
    public static EmployeeType GetEmployeeTypeByDisplayName(string displayName)
    {
        return displayName.ToUpper() switch
        {
            "JUNIOR" => EmployeeType.Junior,
            "TEAMLEAD" => EmployeeType.TeamLead,
            _ => throw new InvalidEnumArgumentException($"Invalid display name: {displayName}")
        };
    }
}