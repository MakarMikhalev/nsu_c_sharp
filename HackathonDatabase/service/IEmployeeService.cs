using HackathonContract.Model;
using HackathonDatabase.model;

namespace HackathonDatabase.service;

public interface IEmployeeService
{
    void SaveEmployeesByTypeAsync(IEnumerable<Employee> entities, EmployeeType employeeType);

    List<EmployeeEntity> GetEmployeeByType(EmployeeType employeeType);
}