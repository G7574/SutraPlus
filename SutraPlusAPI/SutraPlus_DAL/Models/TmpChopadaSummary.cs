using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class TmpChopadaSummary
    {
        public int CompanyId { get; set; }
        public int TotalBags { get; set; }
        public double? TotalWeight { get; set; }
        public decimal TotalBastani { get; set; }
        public decimal TotalPacking { get; set; }
        public decimal Totalw { get; set; }
        public decimal TotalHamali { get; set; }
        public decimal TotalCommission { get; set; }
        public decimal TotalCess { get; set; }
        public decimal TotalTaxable { get; set; }
        public decimal TotalSgst { get; set; }
        public decimal TotalCgst { get; set; }
        public decimal TotalRoff { get; set; }
        public decimal TotalBillAmount { get; set; }
        public decimal? Igst { get; set; }
        public decimal? Others { get; set; }
    }
}
