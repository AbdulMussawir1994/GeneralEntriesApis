using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeneralEntries.Models;

//[Table("Employee", Schema = "public")]
public class Employee
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public int Age { get; set; }
    public decimal Salary { get; set; }

    // UserName is not mapped to the database
    [NotMapped]
    public string UserName { get; set; } = string.Empty;

    [ForeignKey(nameof(ApplicationUser))]
    public string ApplicationUserId { get; set; } = string.Empty;

    public virtual ApplicationUser ApplicationUser { get; set; } = null!;
}
