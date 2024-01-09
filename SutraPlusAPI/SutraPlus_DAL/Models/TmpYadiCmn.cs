using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class TmpYadiCmn
    {
        public long? Fid { get; set; }
        public DateTime Dt { get; set; }
        public long Hid { get; set; }
        public decimal Amt { get; set; }
        public DateTime? Todt { get; set; }
        public decimal? TodtAmt { get; set; }
    }
}
