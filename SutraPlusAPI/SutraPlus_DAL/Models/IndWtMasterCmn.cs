using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class IndWtMasterCmn
    {
        public long CompanyId { get; set; }
        public long LotNo { get; set; }
        public DateTime TrDt { get; set; }
        public double Wt { get; set; }
        public long MId { get; set; }
        public long SrNo { get; set; }
        public bool? TtlWt { get; set; }
        public long? YrId { get; set; }
        public long? KBlNo { get; set; }
        public string? KBlNoC { get; set; }
        public long? EntryId { get; set; }
        public long? EditId { get; set; }
        public string? BuyerCode { get; set; }
        public string? SellerCode { get; set; }
        public decimal? Rate { get; set; }
        public string? PrintNumber { get; set; }
    }
}
