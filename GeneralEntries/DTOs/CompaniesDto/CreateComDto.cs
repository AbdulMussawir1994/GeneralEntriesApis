using System.ComponentModel.DataAnnotations;

namespace GeneralEntries.DTOs.CompaniesDto;

public class CreateComDto
{
    public int CompanyId { get; set; }

    [StringLength(20, MinimumLength = 6, ErrorMessage = "Company name must be at least 6 characters long.")]
    public required string CompanyName { get; set; }
    public required string Country { get; set; }
    public required string City { get; set; }
    public string Branch { get; set; }

    public required int EmployeeId { get; set; }
}
