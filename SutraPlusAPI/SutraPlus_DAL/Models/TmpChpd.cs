using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class TmpChpd
    {
        public long Fid { get; set; }
        public DateTime Tdt { get; set; }
        public long Lt { get; set; }
        public decimal Rt { get; set; }
        public double TtlWt { get; set; }
        public decimal? TtlAmt { get; set; }
        public string? Wt { get; set; }
        public long Bgs { get; set; }
        public long Hid { get; set; }
        public long? TtlBags { get; set; }
        public long? RfBgs { get; set; }
        public string? Mwt { get; set; }
        public long? MNoOfBags { get; set; }
        public string? Mcd { get; set; }
        public double? TtlWtForM { get; set; }
        public long? VBlNo { get; set; }
        public decimal? TtlBast { get; set; }
    }
}
