using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class TmpHdDtl
    {
        public long Fid { get; set; }
        public long Hid { get; set; }
        public DateTime? TrDt { get; set; }
        public string? Prt { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Debit { get; set; }
        public DateTime Fdt { get; set; }
        public DateTime Tdt { get; set; }
        public decimal? TtlD { get; set; }
        public decimal? TtlC { get; set; }
        public decimal? ItsC { get; set; }
        public decimal? ItsD { get; set; }
        public decimal? CMinus { get; set; }
        public decimal? DMinus { get; set; }
        public long? SrNo { get; set; }
        public decimal? FnlD { get; set; }
        public decimal? FnlC { get; set; }
        public decimal? ClsD { get; set; }
        public decimal? ClsC { get; set; }
        public decimal? RunCb { get; set; }
    }
}
