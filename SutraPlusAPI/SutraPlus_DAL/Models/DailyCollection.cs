using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class DailyCollection
    {
        public long CompanyId { get; set; }
        public long Id { get; set; }
        public string? AccountNumber { get; set; }
        public DateTime? TranctDate { get; set; }
        public long LedgerId { get; set; }
        public decimal Amount { get; set; }
        public string? AgentName { get; set; }
    }
}
