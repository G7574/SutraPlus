using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class Voucher
    {
        public long? CompanyId { get; set; }
        public long? LedgerId { get; set; }
        public long? VoucherId { get; set; }
        public long? VoucherNo { get; set; }
        public DateTime? TranctDate { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Debit { get; set; }
        public string? Narration { get; set; }
        public long? CommodityId { get; set; }
        public long? FnoOfBags { get; set; }
        public double? Fweight { get; set; }
        public long? Fvalue { get; set; }
        public decimal? Fpacking { get; set; }
        public decimal? Fhamali { get; set; }
        public decimal? Kvalue { get; set; }
        public decimal? Kpacking { get; set; }
        public decimal? Khamali { get; set; }
        public decimal? KweighmanFee { get; set; }
        public decimal? Kcommission { get; set; }
        public decimal? Kcess { get; set; }
        public decimal? Ksgst { get; set; }
        public decimal? Kcgst { get; set; }
        public decimal? Kroundoff { get; set; }
        public string? PartyInvoiceNumber { get; set; }
        public string? LedgerNameForNarration { get; set; }
        public int? VehicleId { get; set; }
        public long Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? BrandName { get; set; }
        public string? ForEdit { get; set; }
        public int? IsChqPayment { get; set; }
        public int? ToPrint { get; set; }
        public int? Tdstype { get; set; }
        public int? PartyId { get; set; }
        public decimal? Cmnamount { get; set; }
        public string? Inwords { get; set; }
        public int? IsFinalize { get; set; }
        public int? IsDiscountEntry { get; set; }
        public string? EntryGroup { get; set; }
        public int? QuantraType { get; set; }
        public string? GstqunatryName { get; set; }
        public int? IsDperc { get; set; }
        public int? QuantraDone { get; set; }
        public int? IsHide { get; set; }
        public int? IsActive { get; set; }
        public int? IsPayToGarmer { get; set; }
    }
}
