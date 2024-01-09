using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class TmpTdsList
    {
        public long Fid { get; set; }
        public DateTime Dt { get; set; }
        public long Bl { get; set; }
        public decimal Amt { get; set; }
        public long Mid { get; set; }
        public DateTime? Fdt { get; set; }
        public DateTime? Tdt { get; set; }
        public decimal? Cmn { get; set; }
        public decimal? Tds { get; set; }
    }
}
