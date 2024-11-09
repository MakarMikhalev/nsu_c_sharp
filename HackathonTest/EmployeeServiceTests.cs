namespace HackathonTest;

using HackathonDatabase.model;
using HackathonDatabase.service;
using Xunit;
using HackathonDatabase;
using Microsoft.EntityFrameworkCore;

public class EmployeeServiceTests
{
    private readonly EmployeeService _employeeService;
    private readonly ApplicationDbContext _dbContext;

    public EmployeeServiceTests()
    {
        _dbContext = SetupInMemoryDatabase();
        _employeeService = new EmployeeService(_dbContext);
    }

    [Fact]
    public void SaveEmployeesByTypeAsync_ShouldAddNewEmployees_WhenTheyDoNotExist()
    {
        var employeeType = EmployeeType.JUNIOR;
        var employees = ModelFactory.GenerateEmployees(2, "Junior");

        _employeeService.SaveEmployeesByTypeAsync(employees, employeeType);

        AssertEmployeesSaved(employeeType, 2);
    }

    [Fact]
    public void GetEmployeeByType_ShouldReturnEmployees_WhenEmployeeTypeIsMatched()
    {
        var employeeType = EmployeeType.JUNIOR;
        var employees = ModelFactory.GenerateEmployees(3);

        _employeeService.SaveEmployeesByTypeAsync(employees, employeeType);

        var result = _employeeService.GetEmployeeByType(employeeType);

        Assert.Equal(3, result.Count);
        Assert.All(result, e => Assert.Equal(employeeType, e.EmployeeType));
    }

    private ApplicationDbContext SetupInMemoryDatabase()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        var dbContext = new ApplicationDbContext(options);
        dbContext.Database.EnsureCreated();
        return dbContext;
    }

    private void AssertEmployeesSaved(EmployeeType employeeType, int expectedCount)
    {
        var savedEmployees = _dbContext.EmployeeEntities
            .Where(e => e.EmployeeType == employeeType)
            .ToList();

        Assert.Equal(expectedCount, savedEmployees.Count);
        Assert.All(
            savedEmployees,
            e => Assert.Equal(employeeType, e.EmployeeType)
        );
    }
}