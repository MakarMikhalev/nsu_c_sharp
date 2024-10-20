using HackathonContract.Model;
using HackathonDatabase.model;

namespace HackathonDatabase.mapper;

public static class EmployeeMapper
{
    public static EmployeeEntity Entity(Employee employee, EmployeeType employeeType)
    {
        return new EmployeeEntity
        {
            Id = employee.Id,
            Name = employee.Name,
            EmployeeType = employeeType
        };
    }
}