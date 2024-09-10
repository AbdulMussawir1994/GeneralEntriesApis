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


        //TypeAdapterConfig<Employee, EmployeeGetDto>.NewConfig()
        //   .Map(dest => dest.EmployeeName, src => src.EmployeeName)
        //   .Map(dest => dest.ImageUrl, src => src.ImageUrl)
        //   .Map(dest => dest.CvUrl, src => src.CVURL)
        //   .Map(dest => dest.AudioUrl, src => src.AudioCVURL)
        //   .Map(dest => dest.VideoUrl, src => src.VideoCVURL)
        //   .Map(dest => dest.Age, src => src.Age)
        //   .Map(dest => dest.Salary, src => src.Salary)
        //   .Map(dest => dest.ApplicationUserId, src => src.ApplicationUserId)
        //   .Map(dest => dest.UserName, src => src.UserName);

        //TypeAdapterConfig<EmployeeGetDto, Employee>.NewConfig()
        //   .Map(dest => dest.EmployeeName, src => src.EmployeeName)
        //   .Map(dest => dest.ImageUrl, src => src.ImageUrl)
        //   .Map(dest => dest.CVURL, src => src.CvUrl)
        //   .Map(dest => dest.AudioCVURL, src => src.AudioUrl)
        //   .Map(dest => dest.VideoCVURL, src => src.VideoUrl)
        //   .Map(dest => dest.Age, src => src.Age)
        //   .Map(dest => dest.Salary, src => src.Salary)
        //   .Map(dest => dest.ApplicationUsers.Id, src => src.ApplicationUserId)
        //   .Map(dest => dest.ApplicationUsers.UserName, src => src.UserName)
        //   .Ignore(dest => dest.ApplicationUsers.Id);

        //TypeAdapterConfig<CreateEmployeeDto, Employee>.NewConfig();
        //TypeAdapterConfig<CreateEmployeeDto, EmployeeGetDto>.NewConfig();


    }
}