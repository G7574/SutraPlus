using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class PadatalMaster
    {
        public string? PadatalName { get; set; }
        public int CompanyId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? AgentName { get; set; }
        public long LotNo { get; set; }
        public long Bags { get; set; }
        public double Weight { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public string? PurchaseDate { get; set; }
        public DateTime? ActualPurchaseDate { get; set; }
        public int? LedgerId { get; set; }
    }
}
