using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class BillwiseReceipt
    {
        public int? CompanyId { get; set; }
        public decimal? LedgerId { get; set; }
        public decimal? VochId { get; set; }
        public decimal? VochNo { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Narration { get; set; }
        public decimal? ReceiptVochNo { get; set; }
    }
}
