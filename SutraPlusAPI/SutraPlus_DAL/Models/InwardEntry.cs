using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class InwardEntry
    {
        public long Id { get; set; }
        public long? CompanyId { get; set; }
        public long? StockPointId { get; set; }
        public long? CommodityId { get; set; }
        public long? LedgerId { get; set; }
        public long? InwardTypeId { get; set; }
        public decimal? NoOfBags { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? EntryDate { get; set; }
        public long? VehicleId { get; set; }
        public long? BookNo { get; set; }
        public string? StaffName { get; set; }
        public long? ChargeTypeId { get; set; }
    }
}
