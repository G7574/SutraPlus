using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class TmpBl
    {
        public long Fid { get; set; }
        public long Hid { get; set; }
        public DateTime TrDt { get; set; }
        public long LotNo { get; set; }
        public double Wt { get; set; }
        public decimal Rt { get; set; }
        public long Bgs { get; set; }
        public decimal Amt { get; set; }
        public long BlNo { get; set; }
        public string? TtlWtForAMer { get; set; }
        public string? TinNo { get; set; }
        public decimal? Hml { get; set; }
        public decimal? Asf { get; set; }
        public decimal? Rndf { get; set; }
        public decimal? Cmn { get; set; }
        public decimal? Adv { get; set; }
        public string? AltLotNo { get; set; }
        public decimal? Ces { get; set; }
        public decimal? Frght { get; set; }
        public decimal? Kchl { get; set; }
        public string? Str { get; set; }
        public decimal? Pkng { get; set; }
        public decimal? Dalali { get; set; }
        public decimal? Wtf { get; set; }
        public decimal? Vat { get; set; }
        public decimal? Csgt { get; set; }
        public decimal? Sgst { get; set; }
        public decimal? Cgst { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? PrintNumber { get; set; }
    }
}
