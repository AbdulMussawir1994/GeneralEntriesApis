using GeneralEntries.DTOs.CompaniesDto;
using GeneralEntries.Models;

namespace GeneralEntries.Helpers.Data_Structure;

public static class ModelConverter
{
    public static Company DtoToModel(GetCompanyDto getCompanyDto)
    {
        return new Company
        {
            CompanyId = getCompanyDto.CompanyId,
            CompanyName = getCompanyDto.name,
            Country = getCompanyDto.country,
            Branch = getCompanyDto.branch,
            City = getCompanyDto.city,
            EmployeeId = getCompanyDto.empId
        };
    }

    public static GetCompanyDto ModelToDto(Company company)
    {
        return new GetCompanyDto
        {
            CompanyId = company.CompanyId,
            name = company.CompanyName,
            branch = company.Branch,
            city = company.City,
            country = company.Country,
            empId = company.EmployeeId,
            empName = company.Employee?.EmployeeName ?? string.Empty
        };
    }
}
