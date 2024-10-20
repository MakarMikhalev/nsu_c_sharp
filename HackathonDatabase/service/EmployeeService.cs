using HackathonContract.Model;
using HackathonDatabase.mapper;
using HackathonDatabase.model;
using Microsoft.EntityFrameworkCore;

namespace HackathonDatabase.service;

public class EmployeeService
{
    private readonly ApplicationDbContext _applicationDbContext;

    public EmployeeService(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public void SaveEmployeesByTypeAsync(IEnumerable<Employee> entities,
        EmployeeType employeeType)
    {
        var entityIds = entities.Select(e => e.Id).ToList();

        var existingEmployees = _applicationDbContext.EmployeeEntities
            .Where(e => entityIds.Contains(e.Id) && e.EmployeeType == employeeType)
            .ToList();

        var newEmployees = entities
            .Where(e => !existingEmployees.Any(existing => existing.Id == e.Id))
            .Select(e => EmployeeMapper.Entity(e, employeeType))
            .ToList();

        if (newEmployees.Any())
        {
            _applicationDbContext.EmployeeEntities.AddRange(newEmployees);
            _applicationDbContext.SaveChanges();
        }
    }

    public List<EmployeeEntity> GetEmployeeByType(EmployeeType employeeType)
    {
        return _applicationDbContext.EmployeeEntities
            .Where(e => e.EmployeeType == employeeType)
            .ToList();
    }
}