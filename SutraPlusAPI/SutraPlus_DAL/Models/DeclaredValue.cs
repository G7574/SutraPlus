using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class DeclaredValue
    {
        public int? CompanyId { get; set; }
        public DateTime? TranctDate { get; set; }
        public int? CommodityId { get; set; }
        public double? ShorgateWeight { get; set; }
        public double? ClosingWeight { get; set; }
        public decimal? AverageRate { get; set; }
        public decimal? ClosingValue { get; set; }
        public double? PurchaseTotalWeihgt { get; set; }
        public double? SaleTotalWeihgt { get; set; }
    }
}
