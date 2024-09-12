using Duende.IdentityServer.Models;
using System.ComponentModel.DataAnnotations;

namespace GeneralEntries.DTOs
{
    public class UpdateEmployeeDto
    {

        public int EmployeeId { get; set; }  // Required to identify the employee
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Employee name must be at least 6 characters long.")]
        public required string EmployeeName { get; set; }

        [Range(18, 65, ErrorMessage = "Age must be between 18 and 65.")]
        public required int Age { get; set; }

        [Range(1000, 100000, ErrorMessage = "Salary must be between 1,000 and 100,000.")]
        public required decimal Salary { get; set; }

        public required string ApplicationUserId { get; set; }
    }
}
