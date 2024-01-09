using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class DailyMaster
    {
        public long Companyid { get; set; }
        public long Ledgerid { get; set; }
        public DateTime TrDt { get; set; }
        public long? LotNo { get; set; }
        public long? Bgs { get; set; }
        public decimal? Rt { get; set; }
        public long? VNo { get; set; }
        public long? KNo { get; set; }
        public long? MerId { get; set; }
        public long? YrId { get; set; }
        public string? AltLotNo { get; set; }
        public decimal? Adv { get; set; }
        public decimal? Frght { get; set; }
        public decimal? Kchl { get; set; }
        public string? Str { get; set; }
        public decimal? Unl { get; set; }
        public long? VrtId { get; set; }
        public string? Bd { get; set; }
        public string? Vrt { get; set; }
    }
}
