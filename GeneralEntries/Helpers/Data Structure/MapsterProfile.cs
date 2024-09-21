using GeneralEntries.DTOs;
using GeneralEntries.DTOs.CompaniesDto;
using GeneralEntries.Models;
using GeneralEntries.ViewModel;
using Mapster;

namespace GeneralEntries.Helpers.Data_Structure;
public sealed class MapsterProfile : TypeAdapterConfig
{
    public MapsterProfile()
    {

        // User Mapster
        TypeAdapterConfig<RegisterViewModel, RegisterDto>.NewConfig()
         .Map(dest => dest.Fullname, src => src.Username)
         .Map(dest => dest.EmailAddress, src => src.Email)
         .Map(dest => dest.First, src => src.FirstName)
         .Map(dest => dest.Last, src => src.LastName);

        //Employee Mapster
        TypeAdapterConfig<Employee, GetEmployeeDto>.NewConfig()
           .Map(dest => dest.EmployeeName, src => src.EmployeeName)
           .Map(dest => dest.Age, src => src.Age)
           .Map(dest => dest.Salary, src => src.Salary)
           .Map(dest => dest.ApplicationUserId, src => src.ApplicationUserId)
           .Map(dest => dest.UserName, src => src.ApplicationUser.UserName);

        TypeAdapterConfig<GetEmployeeDto, Employee>.NewConfig()
           .Map(dest => dest.EmployeeName, src => src.EmployeeName)
           .Map(dest => dest.Age, src => src.Age)
           .Map(dest => dest.Salary, src => src.Salary)
           .Map(dest => dest.ApplicationUser.Id, src => src.ApplicationUserId)
           .Map(dest => dest.ApplicationUser.UserName, src => src.UserName)
           .Ignore(dest => dest.ApplicationUser.Id);

        TypeAdapterConfig<CreateEmployeeDto, Employee>.NewConfig();
        TypeAdapterConfig<CreateEmployeeDto, GetEmployeeDto>.NewConfig();

        //Company Mapster
        TypeAdapterConfig<Company, GetCompanyDto>.NewConfig()
            .Map(com => com.name, src => src.CompanyName)
            .Map(com => com.country, src => src.Country)
            .Map(com => com.city, src => src.City)
            .Map(com => com.branch, src => src.Branch)
            .Map(com => com.empId, src => src.EmployeeId)
            .Map(com => com.empName, src => src.Employee.EmployeeName);

        TypeAdapterConfig<GetCompanyDto, Company>.NewConfig()
            .Map(com => com.CompanyName, src => src.name)
            .Map(com => com.Country, src => src.country)
            .Map(com => com.Branch, src => src.branch)
            .Map(com => com.City, src => src.city)
            .Map(com => com.Employee.EmployeeId, src => src.empId)
            .Map(com => com.Employee.EmployeeName, src => src.empName)
            .Ignore(com => com.Employee.EmployeeId);

        TypeAdapterConfig<CreateComDto, Company>.NewConfig();
        TypeAdapterConfig<CreateComDto, GetCompanyDto>.NewConfig();

    }
}