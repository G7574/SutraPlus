using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class Tmpyadi
    {
        public long HdId { get; set; }
        public long Fid { get; set; }
        public DateTime TrDt { get; set; }
        public decimal Credit { get; set; }
        public DateTime? Ason { get; set; }
        public int? Days { get; set; }
    }
}
