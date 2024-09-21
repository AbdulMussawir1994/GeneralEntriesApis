using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeneralEntries.Models;

public class ChartsofAccounts
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LedgerId { get; set; }

    public string LedgerName { get; set; } = string.Empty;
    public string HeadofAccount { get; set; } = string.Empty;
    public string NatureofAccount { get; set; } = string.Empty;
    public int? CostCenter { get; set; }
    public decimal? OpeningBalance { get; set; }

    [ForeignKey(nameof(Company))]
    public int CompanyId { get; set; }
    public virtual Company Company { get; set; }

    public virtual ICollection<GeneralEntry> AllEntries { get; private set; } = new List<GeneralEntry>();
}
