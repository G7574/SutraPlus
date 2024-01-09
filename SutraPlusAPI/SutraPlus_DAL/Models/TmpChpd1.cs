using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class TmpChpd1
    {
        public long Hid { get; set; }
        public DateTime Dt { get; set; }
        public decimal Rt { get; set; }
        public long Bags { get; set; }
        public string? Mcd { get; set; }
        public string Wt { get; set; } = null!;
        public long Fid { get; set; }
        public long Lt { get; set; }
        public long? SrNo { get; set; }
        public long? Ord { get; set; }
        public long? Pg { get; set; }
    }
}
