using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class IndWtMasterr
    {
        public long Fid { get; set; }
        public long LotNo { get; set; }
        public DateTime TrDt { get; set; }
        public double Wt { get; set; }
        public long MId { get; set; }
        public long SrNo { get; set; }
        public bool? TtlWt { get; set; }
        public long? YrId { get; set; }
        public long? KBlNo { get; set; }
        public double? Cess { get; set; }
        public double? Dd { get; set; }
        public double? Pkng { get; set; }
    }
}
