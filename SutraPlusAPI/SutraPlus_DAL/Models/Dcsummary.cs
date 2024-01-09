using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class Dcsummary
    {
        public long CompanyId { get; set; }
        public long Id { get; set; }
        public string? LorryNumber { get; set; }
        public string? SenderDetails { get; set; }
        public string? DeliveryDetails { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? ConsignorsName { get; set; }
        public long ConsignorsAddress { get; set; }
        public string? ConsigneesName { get; set; }
        public string? ConsigneesAddress { get; set; }
        public string? FromLocation { get; set; }
        public decimal TotalFrieght { get; set; }
        public decimal Advance { get; set; }
        public decimal Balance { get; set; }
        public string? ToLocation { get; set; }
        public int? Bno { get; set; }
        public string? ReceiverDetails { get; set; }
        public DateTime? Date { get; set; }
    }
}
