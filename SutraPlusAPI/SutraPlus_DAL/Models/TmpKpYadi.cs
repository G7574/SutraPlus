using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class TmpKpYadi
    {
        public long Fid { get; set; }
        public long HdId { get; set; }
        public long Blno { get; set; }
        public decimal Amt { get; set; }
        public DateTime TrDt { get; set; }
    }
}
