using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class TmpTrialBalance
    {
        public int CompanyId { get; set; }
        public DateTime AsOnDate { get; set; }
        public int LedgerId { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        public string? OpeningBalance { get; set; }
    }
}
