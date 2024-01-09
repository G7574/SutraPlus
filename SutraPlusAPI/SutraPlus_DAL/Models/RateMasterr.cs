using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class RateMasterr
    {
        public string Yr { get; set; } = null!;
        public long Mnth { get; set; }
        public decimal Sls { get; set; }
        public decimal VHml { get; set; }
        public decimal VPk { get; set; }
        public decimal Dl { get; set; }
        public decimal Asso { get; set; }
        public decimal KPk { get; set; }
        public decimal Ces { get; set; }
        public decimal KHml { get; set; }
        public decimal Wt { get; set; }
        public decimal? Perbg { get; set; }
        public decimal? AdvRt { get; set; }
        public decimal? RndTo { get; set; }
        public decimal? Unld { get; set; }
        public DateTime? TrDt { get; set; }
    }
}
