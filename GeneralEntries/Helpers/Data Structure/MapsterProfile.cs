using GeneralEntries.DTOs;
using GeneralEntries.Models;
using GeneralEntries.ViewModel;
using Mapster;

namespace GeneralEntries.Helpers.Data_Structure;
public class MapsterProfile : TypeAdapterConfig
{
    public MapsterProfile()
    {
        TypeAdapterConfig<RegisterViewModel, RegisterDto>.NewConfig()
         .Map(dest => dest.Fullname, src => src.Username)
         .Map(dest => dest.EmailAddress, src => src.Email)
         .Map(dest => dest.First, src => src.FirstName)
         .Map(dest => dest.Last, src => src.LastName);


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


    }
}