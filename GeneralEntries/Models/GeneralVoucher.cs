using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeneralEntries.Models;

public class GeneralVoucher
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int GeneralVoucherId { get; set; }

    public string Voucher { get; set; } = string.Empty;
    public DateTime Vdate { get; set; }

    public virtual ICollection<GeneralEntry> GenEntries { get; private set; } = new List<GeneralEntry>();
}
