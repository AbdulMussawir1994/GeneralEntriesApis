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

        var serviceResponse = new ServiceResponse<IEnumerable<GetEmployeeDto>>();

        try
        {
            var employees = await _dbContextClass.Employees
                                                 .Include(e => e.ApplicationUser)
                                                 .AsNoTracking()
                                                 .AsSplitQuery()
                                                 .ToListAsync(cancellationToken);

            //var res = await _dbContextClass.Employees
            //    .GroupBy(e => e.Age)
            //    .Select(group => new
            //    {
            //        Age = group.Key,               // Age is the group key
            //        EmployeesCount = group.Count()  // Count the number of employees in each age group
            //    })
            //    .ToListAsync();

            //var result = await _dbContextClass.Employees
            //                                .Include(x => x.ApplicationUser)
            //                                .AsNoTracking()
            //                                .IgnoreQueryFilters()
            //                                .AsSplitQuery() // Split query improves performance for large includes
            //                                .ToListAsync(cancellationToken);

            if (employees.Any())
            {
                serviceResponse.Value = employees.Adapt<IEnumerable<GetEmployeeDto>>();
                serviceResponse.Status = true;
                serviceResponse.Message = "Employee list fetched successfully.";
            }
            else
            {
                serviceResponse.Value = Enumerable.Empty<GetEmployeeDto>();
                serviceResponse.Status = false;
                serviceResponse.Message = "No employees found.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching employees.");
            serviceResponse.Status = false;
            serviceResponse.Message = "An error occurred.";
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<GetEmployeeDto>> AddNewEmployeeAsync(CreateEmployeeDto model, CancellationToken cancellationToken)
    {
        var serviceResponse = new ServiceResponse<GetEmployeeDto>();

        try
        {
            var newEmployee = model.Adapt<Employee>();

            await _dbContextClass.Employees.AddAsync(newEmployee, cancellationToken); // Use AddAsync for async behavior

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
                                           .AsTracking() // Use tracking for updates
                                           .Include(x => x.ApplicationUser)
                                           .SingleOrDefaultAsync(c => c.EmployeeId == model.EmployeeId, cancellationToken);

            if (emp is null)
            {
                serviceResponse.Status = false;
                serviceResponse.Message = "Employee not found.";
                return serviceResponse;
            }

            emp = model.Adapt(emp); // Map updates

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
                                           .AsNoTracking()
                                           .Include(x => x.ApplicationUser)
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
            _logger.LogError(ex, "Delete Employee from Employee Layer Error Occurred");
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<GetEmployeeDto>> PatchEmployeeAsync(int Id, string empName, CancellationToken cancellationToken)
    {
        var serviceResponse = new ServiceResponse<GetEmployeeDto>();

        try
        {
            var emp = await _dbContextClass.Employees
                                          .AsTracking()
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
