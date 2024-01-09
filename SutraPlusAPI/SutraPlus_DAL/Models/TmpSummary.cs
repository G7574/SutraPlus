using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class TmpSummary
    {
        public long? Fid { get; set; }
        public long? HdId { get; set; }
        public decimal? Obcr { get; set; }
        public decimal? Obdr { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Cbcr { get; set; }
        public decimal? Cbdr { get; set; }
        public DateTime? Ason { get; set; }
    }
}
