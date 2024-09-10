using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeneralEntries.Models;

public class GeneralEntry
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EntryId { get; set; }

    [ForeignKey(nameof(GeneralVoucher))]
    public int GeneralVoucherId { get; set; }
    public virtual GeneralVoucher GeneralVoucher { get; set; } = null!; // Non-nullable reference type

    public string VNumber { get; set; } = string.Empty;
    public string RelateCompany { get; set; } = string.Empty;
    public string RelateLedger { get; set; } = string.Empty;
    public string Narration { get; set; } = string.Empty;
    public string CostCenter { get; set; } = string.Empty;
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }

    [ForeignKey(nameof(ChartsofAccounts))]
    public int ChartId { get; set; }
    public virtual ChartsofAccounts Chart { get; set; } = null!;
}
