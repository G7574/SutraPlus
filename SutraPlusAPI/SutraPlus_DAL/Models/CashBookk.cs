using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class CashBookk
    {
        public long Fid { get; set; }
        public long SrNo { get; set; }
        public long HdId { get; set; }
        public long? VBlNo { get; set; }
        public long? KBlNo { get; set; }
        public string Prt { get; set; } = null!;
        public DateTime TrDt { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Debit { get; set; }
        public long EntrTp { get; set; }
        public bool? FarPay { get; set; }
        public long? QId { get; set; }
        public bool? Paid { get; set; }
        public decimal? VikriBastani { get; set; }
        public decimal? VikriPacking { get; set; }
        public decimal? VikriHamali { get; set; }
        public decimal? VikriWtf { get; set; }
        public decimal? VikriAsf { get; set; }
        public decimal? VikriTaxable { get; set; }
        public decimal? VikriVat { get; set; }
        public decimal? VikriCommission { get; set; }
        public decimal? VikriCess { get; set; }
        public decimal? VikriRndf { get; set; }
        public long? VikriBags { get; set; }
        public double? VikriWt { get; set; }
        public long? VikriBlNo { get; set; }
        public long? VMin { get; set; }
        public long? VMax { get; set; }
    }
}
