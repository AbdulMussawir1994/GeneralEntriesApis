using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Moq;
using GeneralEntries.ContextClass;
using GeneralEntries.Models;
using GeneralEntries.RepositoryLayer.InterfaceClass;
using GeneralEntries.RepositoryLayer.ServiceClass;
using Microsoft.AspNetCore.Http;

public class EmployeeTesting
{
    private readonly Mock<IConfiguration> _mockIconfiguration;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly Mock<ILogger<EmployeeLayer>> _mockLogger;
    private readonly IEmployeeLayer _employeeService;
    private readonly DbContextClass _dbContext;

    private readonly List<Employee> _employeeSeedData;

    public EmployeeTesting()
    {
        var options = new DbContextOptionsBuilder<DbContextClass>()
            .UseInMemoryDatabase(databaseName: "EmployeeTestDB")
            .Options;

        _dbContext = new DbContextClass(options);
        _mockLogger = new Mock<ILogger<EmployeeLayer>>();
        _mockIconfiguration = new Mock<IConfiguration>();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

        _employeeService = new EmployeeLayer(_mockIconfiguration.Object, _mockLogger.Object, _dbContext);

        // Seed test data once to avoid multiple database calls
        _employeeSeedData = new List<Employee>
        {
            new Employee { EmployeeName = "Raheel", Age = 29, Salary = 100000, ApplicationUserId = "a27bcbae-b26c-4ce9-ab22-fc9c1f85d2d5" },
            new Employee { EmployeeName = "Mussawir", Age = 25, Salary = 150000 , ApplicationUserId = "a27bcbae-b26c-4ce9-ab22-fc9c1f85d2d5"},
            new Employee { EmployeeName = "Azhaan", Age = 35, Salary = 60000, ApplicationUserId = "a27bcbae-b26c-4ce9-ab22-fc9c1f85d2d5" }
        };

        _dbContext.Employees.AddRange(_employeeSeedData);
        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEmployee_WhenEmployeeExists()
    {
        // Arrange
        Employee expectedEmployee = _employeeSeedData.First(e => e.EmployeeName == "Raheel");
        CancellationToken cancellationToken = CancellationToken.None;

        // Act
        var result = await _employeeService.GetIdByAsync(expectedEmployee.EmployeeId, cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedEmployee.EmployeeName, result.Value.EmployeeName);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowOperationCanceledException_WhenTokenIsCancelled()
    {
        // Arrange
        Employee expectedEmployee = _employeeSeedData.First(e => e.EmployeeName == "Raheel");

        // Create a CancellationTokenSource and cancel it
        using (var cts = new CancellationTokenSource())
        {
            cts.Cancel(); // Cancel the operation
            var cancellationToken = cts.Token;

            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            {
                await _employeeService.GetIdByAsync(expectedEmployee.EmployeeId, cancellationToken);
            });
        }
    }

    [Fact]
    public async Task GetByIdAsync_ShouldHandleNoEmployeeFound()
    {
        // Arrange
        int nonExistentEmployeeId = 9999;
        CancellationToken cancellationToken = CancellationToken.None;

        // Act
        var result = await _employeeService.GetIdByAsync(nonExistentEmployeeId, cancellationToken);

        // Assert
        Assert.Null(result);
    }
}