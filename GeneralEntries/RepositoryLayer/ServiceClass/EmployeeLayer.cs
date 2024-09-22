using GeneralEntries.ContextClass;
using GeneralEntries.DTOs;
using GeneralEntries.DTOs.CompaniesDto;
using GeneralEntries.Helpers.Response;
using GeneralEntries.Models;
using GeneralEntries.RepositoryLayer.InterfaceClass;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace GeneralEntries.RepositoryLayer.ServiceClass;

public class EmployeeLayer : IEmployeeLayer
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmployeeLayer> _logger;
    private readonly DbContextClass _dbContextClass;

    public EmployeeLayer(IConfiguration configuration, ILogger<EmployeeLayer> logger, DbContextClass dbContextClass)
    {
        _configuration = configuration;
        _logger = logger;
        _dbContextClass = dbContextClass;
    }

    public async Task<ServiceResponse<IEnumerable<GetEmployeeDto>>> GetListAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("GetListAsync EmployeeLayer is Calling");
        var serviceResponse = new ServiceResponse<IEnumerable<GetEmployeeDto>>();
        try
        {
            var result = await _dbContextClass.Employees
                                              .Include(x => x.ApplicationUser)
                                              .AsNoTracking()
                                              .IgnoreQueryFilters()
                                             // .AsQueryable()
                                              //.AsSplitQuery()
                                              .ToListAsync(cancellationToken);

            if (result.Any())
            {
                serviceResponse.Value = result.Adapt<IEnumerable<GetEmployeeDto>>();
                serviceResponse.Message = "Fetched all employee records successfully.";
            }
            else
            {
                serviceResponse.Value = Enumerable.Empty<GetEmployeeDto>();
                serviceResponse.Message = "No employee records found.";
            }
            serviceResponse.Status = true;

        }
        catch (OperationCanceledException)
        {
            serviceResponse.Status = false;
            serviceResponse.Message = "The operation was canceled.";
            _logger.LogWarning("Employee query operation was canceled.");
        }
        catch (Exception ex)
        {
            serviceResponse.Status = false;
            serviceResponse.Message = $"An error occurred: {ex.Message}";
            _logger.LogError(ex, "Error occurred while fetching employee data.");
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<GetEmployeeDto>> AddNewEmployeeAsync(CreateEmployeeDto model, CancellationToken cancellationToken)
    {
        var serviceResponse = new ServiceResponse<GetEmployeeDto>();

        try
        {
            var newEmployee = model.Adapt<Employee>(); // Assuming Mapster or other mapper

            _dbContextClass.Employees.Add(newEmployee);

            var result = await _dbContextClass.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                serviceResponse.Status = true;
                serviceResponse.Message = "Employee added successfully.";
                serviceResponse.Value = newEmployee.Adapt<GetEmployeeDto>();
            }
            else
            {
                serviceResponse.Status = false;
                serviceResponse.Message = "Failed to add new employee.";
            }
        }
        catch (Exception ex)
        {
            serviceResponse.Status = false;
            serviceResponse.Message = $"An error occurred: {ex.Message}";
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<GetEmployeeDto>> UpdateEmployeeAsync(CreateEmployeeDto model, CancellationToken cancellationToken)
    {
        var serviceResponse = new ServiceResponse<GetEmployeeDto>();

        try
        {
            var emp = await _dbContextClass.Employees
                                           .Include(x => x.ApplicationUser)
                                           .SingleOrDefaultAsync(c => c.EmployeeId == model.EmployeeId, cancellationToken);

            if (emp == null)
            {
                serviceResponse.Status = false;
                serviceResponse.Message = "Employee not found.";
                return serviceResponse;
            }

            emp.EmployeeName = model.EmployeeName;
            emp.Salary = model.Salary;
            emp.Age = model.Age;
            emp.ApplicationUserId = model.ApplicationUserId;

            await _dbContextClass.SaveChangesAsync(cancellationToken);

            serviceResponse.Status = true;
            serviceResponse.Message = "Employee updated successfully.";
            serviceResponse.Value = emp.Adapt<GetEmployeeDto>();
        }
        catch (Exception ex)
        {
            serviceResponse.Status = false;
            serviceResponse.Message = $"An error occurred: {ex.Message}";
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<GetEmployeeDto>> GetIdByAsync(int Id, CancellationToken cancellationToken)
    {
        var serviceResponse = new ServiceResponse<GetEmployeeDto>();

        try
        {
            var emp = await _dbContextClass.Employees
                                           .Include(x => x.ApplicationUser)
                                           .AsNoTracking()
                                           .SingleOrDefaultAsync(x => x.EmployeeId == Id, cancellationToken);

            if (emp == null)
            {
                serviceResponse.Status = false;
                serviceResponse.Message = "Employee not found.";
                return serviceResponse;
            }

            serviceResponse.Value = emp.Adapt<GetEmployeeDto>();
            serviceResponse.Status = true;
            serviceResponse.Message = "Fetched successfully.";
        }
        catch (Exception ex)
        {
            serviceResponse.Status = false;
            serviceResponse.Message = $"An error occurred: {ex.Message}";
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<bool>> DeleteByIdAsync(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Delete EmployeeLayer is Calling");
        var serviceResponse = new ServiceResponse<bool>();

        try
        {
            var emp = await _dbContextClass.Employees.FindAsync(new object[] { id }, cancellationToken);

            if (emp == null)
            {
                serviceResponse.Status = false;
                serviceResponse.Message = "Employee not found.";
                return serviceResponse;
            }

            _dbContextClass.Employees.Remove(emp);
            await _dbContextClass.SaveChangesAsync(cancellationToken);

            serviceResponse.Status = true;
            serviceResponse.Message = "Deleted successfully.";
        }
        catch (Exception ex)
        {
            serviceResponse.Status = false;
            serviceResponse.Message = $"An error occurred: {ex.Message}";
            _logger.LogError("Delete Employee from Employee Layer Error Occur : Message: " + ex.Message);
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<GetEmployeeDto>> PatchEmployeeAsync(int Id, string empName, CancellationToken cancellationToken)
    {
        var serviceResponse = new ServiceResponse<GetEmployeeDto>();

        try
        {
            var emp = await _dbContextClass.Employees
                                          .Include(x => x.ApplicationUser)
                                          .SingleOrDefaultAsync(c => c.EmployeeId == Id, cancellationToken);

            if (emp == null)
            {
                serviceResponse.Status = false;
                serviceResponse.Message = "Employee not found.";
                return serviceResponse;
            }

            if (emp.EmployeeName != empName)
            {
                emp.EmployeeName = empName;
                await _dbContextClass.SaveChangesAsync(cancellationToken);

                serviceResponse.Status = true;
                serviceResponse.Message = "Employee name updated successfully.";
                serviceResponse.Value = emp.Adapt<GetEmployeeDto>();
            }
            else
            {
                serviceResponse.Status = true;
                serviceResponse.Message = "Employee name is already up-to-date.";
                serviceResponse.Value = emp.Adapt<GetEmployeeDto>();
            }
        }
        catch (Exception ex)
        {
            serviceResponse.Status = false;
            serviceResponse.Message = $"An error occurred: {ex.Message}";
        }

        return serviceResponse;
    }

    //private static string CheckNameStrength(string pass)
    //{
    //    StringBuilder sb = new StringBuilder();

    //    if (pass.Length < 6)
    //        sb.Append("Minimum password length should be 6" + Environment.NewLine);

    //    return sb.ToString();
    //}
}
