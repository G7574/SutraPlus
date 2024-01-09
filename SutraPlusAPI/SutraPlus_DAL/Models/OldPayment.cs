using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class OldPayment
    {
        public long? HdId { get; set; }
        public long? Vno { get; set; }
        public decimal? Debit { get; set; }
        public long? Fid { get; set; }
        public DateTime? TrDt { get; set; }
    }
}
