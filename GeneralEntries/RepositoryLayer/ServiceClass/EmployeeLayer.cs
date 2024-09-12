using GeneralEntries.ContextClass;
using GeneralEntries.DTOs;
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

    public async Task<ServiceResponse<IEnumerable<GetEmployeeDto>>> GetListAsync()
    {
        var serviceResponse = new ServiceResponse<IEnumerable<GetEmployeeDto>>();
        try
        {
            var result = await _dbContextClass.Employees.Include(x=>x.ApplicationUser).IgnoreQueryFilters().AsNoTracking().AsQueryable().ToListAsync();

            if(result.Count > 0)
            {
                serviceResponse.Value = result.Adapt<IEnumerable<GetEmployeeDto>>();
                serviceResponse.Status = true;
                serviceResponse.Message = "Fetched all employee records successfully.";
            }

            serviceResponse.Status = true;
            serviceResponse.Message = "No any employee record.";

        }
        catch (Exception ex)
        {
            serviceResponse.Status = false;
            serviceResponse.Message = ex.Message;
        }
        return serviceResponse;
    }

    public async Task<ServiceResponse<GetEmployeeDto>> AddNewEmployeeAsync(CreateEmployeeDto model)
    {
        var serviceResponse = new ServiceResponse<GetEmployeeDto>();

        try
        {
            //var passMessage = CheckNameStrength(model.EmployeeName);

            //if (!string.IsNullOrEmpty(passMessage))
            //{
            //    serviceResponse.Message = passMessage;
            //    serviceResponse.Status = false;
            //    return serviceResponse;
            //}

            var newEmployee = model.Adapt<Employee>(); // Assuming Mapster or other mapper

            _dbContextClass.Employees.Add(newEmployee);

            var result = await _dbContextClass.SaveChangesAsync();

            if (result > 0)
            {
                serviceResponse.Status = true;
                serviceResponse.Message = "Employee added successfully.";
                serviceResponse.Value = newEmployee.Adapt<GetEmployeeDto>();
                return serviceResponse;
            }

            serviceResponse.Status = false;
            serviceResponse.Message = "Failed to add new employee to the database.";
            return serviceResponse;
        }
        catch (Exception ex)
        {
            serviceResponse.Status = false;
            serviceResponse.Message = ex.InnerException.Message;
        }
        return serviceResponse;
    }

    public async Task<ServiceResponse<GetEmployeeDto>> UpdateEmployeeAsync(CreateEmployeeDto model)
    {
        var serviceResponse = new ServiceResponse<GetEmployeeDto>();

        try
        {
            // Find the employee in the database
            var emp = await _dbContextClass.Employees.Include(x=> x.ApplicationUser)
                                           .FirstOrDefaultAsync(c => c.EmployeeId == model.EmployeeId);

            if (emp is null)
            {
                serviceResponse.Status = false;
                serviceResponse.Message = "Employee not found.";
                return serviceResponse;
            }

            // Update entity properties directly
            emp.EmployeeName = model.EmployeeName;
            emp.Salary = model.Salary;
            emp.Age = model.Age;
            emp.ApplicationUserId = model.ApplicationUserId;

            await _dbContextClass.SaveChangesAsync();

            //// Reload the entity from the database to get the latest values
            //await _dbContextClass.Entry(emp).ReloadAsync();

            serviceResponse.Status = true;
            serviceResponse.Message = "Employee updated successfully.";
            serviceResponse.Value = emp.Adapt<GetEmployeeDto>();
        }
        catch (Exception ex)
        {
            serviceResponse.Status = false;
            serviceResponse.Message = ex.InnerException.Message;
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<GetEmployeeDto>> GetIdByAsync(int Id)
    {
        var serviceResponse = new ServiceResponse<GetEmployeeDto>();

        try
        {
            // Fetch employee data including related ApplicationUser
            var emp = await _dbContextClass.Employees
                                           .Include(x => x.ApplicationUser)
                                           .AsNoTracking()
                                           .SingleOrDefaultAsync(x => x.EmployeeId == Id);  // Unique employee Id ensures single record

            if (emp == null)
            {
                serviceResponse.Status = false;
                serviceResponse.Message = "Employee not found.";
                return serviceResponse;
            }

            serviceResponse.Value = emp.Adapt<GetEmployeeDto>();
            serviceResponse.Status = true;
            serviceResponse.Message = "Fetched Successfully.";
        }
        catch (Exception ex)
        {
            serviceResponse.Status = false;
            serviceResponse.Message = $"An error occurred: {ex.Message}";
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<bool>> DeleteByIdAsync(int id)
    {
        var serviceResponse = new ServiceResponse<bool>();

        try
        {
            var emp = await _dbContextClass.Employees.FindAsync(id);

            if (emp == null)
            {
                serviceResponse.Status = false;
                serviceResponse.Message = "Employee not found.";
                return serviceResponse;
            }

            // Remove the employee and save changes
            _dbContextClass.Employees.Remove(emp);
            await _dbContextClass.SaveChangesAsync();

            serviceResponse.Status = true;
            serviceResponse.Message = "Deleted successfully.";
        }
        catch (Exception ex)
        {
            serviceResponse.Status = false;
            serviceResponse.Message = $"An error occurred: {ex.Message}";
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<GetEmployeeDto>> PatchEmployeeAsync(int Id, string empName)
    {
        var serviceResponse = new ServiceResponse<GetEmployeeDto>();

        try
        {
            // Fetch the actual Employee entity by ID
            var emp = await _dbContextClass.Employees.Include(x => x.ApplicationUser)
                                          .FirstOrDefaultAsync(c => c.EmployeeId == Id);

            if (emp == null)
            {
                serviceResponse.Status = false;
                serviceResponse.Message = "Employee not found.";
                return serviceResponse;
            }

            // Only update if the EmployeeName is different
            if (emp.EmployeeName != empName)
            {
                emp.EmployeeName = empName; 

                await _dbContextClass.SaveChangesAsync();

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
