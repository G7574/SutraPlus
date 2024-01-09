using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class DclineItem
    {
        public long Bno { get; set; }
        public long CompanyId { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? MethodOfPacking { get; set; }
        public long Id { get; set; }
        public string? Description { get; set; }
        public string? PrivateMark { get; set; }
        public decimal ActualWeightKg { get; set; }
        public decimal ChangedWeightKg { get; set; }
        public decimal Total { get; set; }
        public decimal Freight { get; set; }
        public DateTime? Date { get; set; }
    }
}
