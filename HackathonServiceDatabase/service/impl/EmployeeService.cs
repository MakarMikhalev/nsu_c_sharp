using HackathonContract.Model;
using HackathonDatabase.model;

namespace HackathonDatabase.service;

public class EmployeeService(ApplicationDbContext applicationDbContext) : IEmployeeService
{
    public void SaveEmployeesByTypeAsync(IEnumerable<Employee> entities,
        EmployeeType employeeType)
    {
        var entityIds = entities.Select(e => e.Id).ToList();

        var existingEmployees = applicationDbContext.EmployeeEntities
            .Where(e => entityIds.Contains(e.Id) && e.EmployeeType == employeeType)
            .ToList();

        var newEmployees = entities
            .Where(e => !existingEmployees.Any(existing => existing.Id == e.Id))
            .Select(e => new EmployeeEntity
                {
                    Id = e.Id,
                    Name = e.Name,
                    EmployeeType = employeeType
                }
            )
            .ToList();

        if (newEmployees.Any())
        {
            applicationDbContext.EmployeeEntities.AddRange(newEmployees);
            applicationDbContext.SaveChanges();
        }
    }
}