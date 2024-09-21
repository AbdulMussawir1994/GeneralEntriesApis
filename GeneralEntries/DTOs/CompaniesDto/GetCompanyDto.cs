namespace GeneralEntries.DTOs.CompaniesDto;

public record struct GetCompanyDto(int CompanyId, string name, string city, string branch, string country, string empId, string empName);
