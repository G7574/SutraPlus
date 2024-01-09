using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class TmpCshbk
    {
        public DateTime? Dt { get; set; }
        public decimal? Op { get; set; }
        public string? Prt { get; set; }
        public decimal? Cr { get; set; }
        public decimal? Db { get; set; }
        public decimal? Clb { get; set; }
        public DateTime? Fdt { get; set; }
        public DateTime? Tdt { get; set; }
        public long? Fid { get; set; }
        public long? Srno { get; set; }
    }
}
