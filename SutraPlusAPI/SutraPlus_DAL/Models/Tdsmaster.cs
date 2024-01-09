using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class Tdsmaster
    {
        public int? CompanyId { get; set; }
        public int? LedgerId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public decimal ToTalCommission { get; set; }
        public decimal TdsAmount { get; set; }
        public string TdsType { get; set; } = null!;
        public DateTime? SavedDate { get; set; }
    }
}
