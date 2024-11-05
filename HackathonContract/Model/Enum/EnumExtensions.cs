using System.ComponentModel;
using System.Reflection;
using HackathonDatabase.model;

namespace HackathonContract.Model.Enum;

public static class EnumExtensions
{
    public static EmployeeType GetEmployeeTypeByDisplayName(string displayName)
    {
        foreach (var field in typeof(EmployeeType).GetFields())
        {
            var attribute = field.GetCustomAttribute<DisplayNameAttribute>();
            if (attribute != null &&
                attribute.Name.Equals(displayName, StringComparison.OrdinalIgnoreCase))
            {
                return (EmployeeType)field.GetValue(null);
            }
        }
        throw new InvalidEnumArgumentException();
    }
}