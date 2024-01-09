using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class TmpReceivable
    {
        public int CompanyId { get; set; }
        public long LedgerId { get; set; }
        public DateTime TranctDate { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
    }
}
