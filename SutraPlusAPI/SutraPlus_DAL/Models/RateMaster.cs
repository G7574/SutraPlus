using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class RateMaster
    {
        public string Yr { get; set; } = null!;
        public long Mnth { get; set; }
        public decimal Sls { get; set; }
        public decimal VHml { get; set; }
        public decimal VPk { get; set; }
        public decimal Dl { get; set; }
        public decimal Asso { get; set; }
        public decimal KPk { get; set; }
        public decimal Ces { get; set; }
        public decimal KHml { get; set; }
        public decimal Wt { get; set; }
        public decimal? Perbg { get; set; }
        public decimal? AdvRt { get; set; }
        public decimal? RndTo { get; set; }
        public decimal? Unld { get; set; }
        public DateTime? TrDt { get; set; }
        public decimal? Wtf { get; set; }
        public decimal? Cess { get; set; }
        public decimal? Cmn { get; set; }
        public int? ItId { get; set; }
        public int? Anfhml { get; set; }
        public int? Bodhfhml { get; set; }
        public int? Bagfhml { get; set; }
        public int? Asf { get; set; }
        public int? Plstfhml { get; set; }
        public int? Anpkng { get; set; }
        public int? Bagpkng { get; set; }
        public int? Bodhpkng { get; set; }
        public int? Plstpkng { get; set; }
        public int? Anfwtf { get; set; }
        public int? Bagfwtf { get; set; }
        public int? Bodhfwtf { get; set; }
        public int? Plstfwtf { get; set; }
        public int? Vat { get; set; }
    }
}
