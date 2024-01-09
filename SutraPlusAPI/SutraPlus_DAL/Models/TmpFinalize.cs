using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class TmpFinalize
    {
        public int? CompanyId { get; set; }
        public int Id { get; set; }
        public int? LedgerId { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Debit { get; set; }
        public string? Particulars { get; set; }
        public string? AccountName { get; set; }
        public string? ActualLedgerName { get; set; }
    }
}
