using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class Expnss
    {
        public long Fid { get; set; }
        public DateTime TrDt { get; set; }
        public long Blno { get; set; }
        public decimal Adv { get; set; }
        public decimal Bamt { get; set; }
        public decimal Exp1 { get; set; }
        public decimal Exp2 { get; set; }
        public decimal Exp3 { get; set; }
        public decimal? Advrt { get; set; }
        public decimal? Blttl { get; set; }
        public string? Prt1 { get; set; }
        public string? Prt2 { get; set; }
        public string? Prt3 { get; set; }
    }
}
