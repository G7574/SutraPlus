using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class BagWeight
    {
        public long? id { get; set; }
        public int? CompanyId { get; set; }
        public int? LedgerId { get; set; }
        public int? VoucherId { get; set; }
        public int? VoucherNumber { get; set; }
        public int? LotNo { get; set; }
        public double? BagWeight1 { get; set; }
        public DateTime? TranctDate { get; set; }
        public string? BuyerCode { get; set; }
        public long? VikriSaleBillNo { get; set; }
    }
}
