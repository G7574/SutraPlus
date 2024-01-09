using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class TmpLedger
    {
        public long CompanyId { get; set; }
        public long Ledgerid { get; set; }
        public long Sno { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime? TranctDate { get; set; }
        public string Narration { get; set; } = null!;
        public decimal? Credit { get; set; }
        public decimal? Debit { get; set; }
    }
}
