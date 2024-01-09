using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class TmpWtBill
    {
        public long Fid { get; set; }
        public DateTime Dt { get; set; }
        public long HdD { get; set; }
        public string Lots { get; set; } = null!;
        public long? LotNo { get; set; }
    }
}
