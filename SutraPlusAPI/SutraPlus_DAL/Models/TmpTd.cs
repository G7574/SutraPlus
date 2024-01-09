using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class TmpTd
    {
        public int? CompanyId { get; set; }
        public int? LedgerId { get; set; }
        public decimal Amount { get; set; }
        public decimal Tds { get; set; }
        public string? Tdstitle { get; set; }
        public string? Tdstype { get; set; }
    }
}
