using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class TmpTaxSlab
    {
        public int CompanyId { get; set; }
        public decimal ZeroValue { get; set; }
        public decimal ZeroTax { get; set; }
        public decimal FiveValue { get; set; }
        public decimal FiveTaxSgst { get; set; }
        public decimal FiveTaxCgst { get; set; }
        public decimal TwelveValue { get; set; }
        public decimal TwelveTaxSgst { get; set; }
        public decimal TwelveTaxCgst { get; set; }
        public decimal EighteenValue { get; set; }
        public decimal EighteenTaxSgst { get; set; }
        public decimal EighteenTaxCgst { get; set; }
        public decimal T8value { get; set; }
        public decimal T8sgsttax { get; set; }
        public decimal T8cgsttax { get; set; }
        public decimal? FiveTaxIgst { get; set; }
        public decimal? EighteenTaxIgst { get; set; }
        public decimal? TwlveTaxIgst { get; set; }
        public decimal? T8taxIgst { get; set; }
        public double? Quantity { get; set; }
    }
}
