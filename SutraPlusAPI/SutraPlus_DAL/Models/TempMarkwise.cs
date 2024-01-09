using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class TempMarkwise
    {
        public int CompanyId { get; set; }
        public string? Mark { get; set; }
        public long Bags { get; set; }
        public decimal Weight { get; set; }
        public decimal Batani { get; set; }
        public DateTime TranctDate { get; set; }
        public decimal MinRate { get; set; }
        public decimal MaxRate { get; set; }
        public decimal BastaniAverage { get; set; }
        public decimal Expense { get; set; }
        public decimal Total { get; set; }
        public decimal FinalAverage { get; set; }
    }
}
