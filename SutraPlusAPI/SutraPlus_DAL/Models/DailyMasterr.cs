using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class DailyMasterr
    {
        public long Fid { get; set; }
        public long HdId { get; set; }
        public DateTime TrDt { get; set; }
        public long? LotNo { get; set; }
        public long? Bgs { get; set; }
        public decimal? Rt { get; set; }
        public long? VNo { get; set; }
        public long? MerId { get; set; }
        public long? YrId { get; set; }
        public string? AltLotNo { get; set; }
        public decimal? Adv { get; set; }
        public decimal? Frght { get; set; }
        public decimal? Kchl { get; set; }
        public string? Str { get; set; }
        public long? Unl { get; set; }
    }
}
