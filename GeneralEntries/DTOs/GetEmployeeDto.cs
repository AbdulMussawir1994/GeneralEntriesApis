namespace GeneralEntries.DTOs;

public record struct GetEmployeeDto(int EmployeeId, string UserName, string EmployeeName, int Age, decimal Salary, string ApplicationUserId);
