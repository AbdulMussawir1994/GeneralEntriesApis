using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeneralEntries.Models;

public class Company
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CompanyId { get; set; }

    public string CompanyName { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;

    [ForeignKey(nameof(Employee))]
    public int EmployeeId { get; set; } 
    public virtual Employee Employee { get; set; } = null!; // Non-nullable reference type

    public virtual ICollection<ChartsofAccounts> Charts { get; private set; } = new List<ChartsofAccounts>();
}
