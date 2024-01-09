using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class TmpCessWk
    {
        public long? Fid { get; set; }
        public string? Dt { get; set; }
        public double? Wt { get; set; }
        public long? Bgs { get; set; }
        public decimal? Amt { get; set; }
        public decimal? Cess { get; set; }
        public decimal? WtFee { get; set; }
        public string? BillNo { get; set; }
    }
}
