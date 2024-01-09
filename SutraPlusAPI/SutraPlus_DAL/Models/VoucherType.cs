using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class VoucherType
    {
        public long VoucherId { get; set; }
        public string? VoucherName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? TransType { get; set; }
        public int? MergeBillNo { get; set; }
    }
}
