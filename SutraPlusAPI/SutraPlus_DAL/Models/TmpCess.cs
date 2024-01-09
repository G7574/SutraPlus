using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class TmpCess
    {
        public long Fid { get; set; }
        public DateTime Fdt { get; set; }
        public DateTime Todt { get; set; }
        public DateTime Trdt { get; set; }
        public long Blno { get; set; }
        public long Mid { get; set; }
        public decimal Camt { get; set; }
        public long Bgs { get; set; }
        public decimal Wamt { get; set; }
        public decimal? BastForKBl { get; set; }
        public long? Vmin { get; set; }
        public long? Vmax { get; set; }
        public double? Wt { get; set; }
    }
}
