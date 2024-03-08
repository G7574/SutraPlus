using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SutraPlus_DAL.Models
{
    [Table("Inventory")]
    public partial class Inventory
    {        
        public int _Id { get; set; }
        public long? CompanyId { get; set; }
        public long? VochType { get; set; }
        public string? InvoiceType { get; set; }
        public long? VochNo { get; set; }
        public long? LedgerId { get; set; }
        public long? CommodityId { get; set; }
        public string? CommodityName { get; set; }
        public DateTime? TranctDate { get; set; }
        public string? PoNumber { get; set; }
        public string? EwaybillNo { get; set; }
        public long? LotNo { get; set; }
        public double? WeightPerBag { get; set; }
        public double? NoOfBags { get; set; }
        public double? NoOfDocra { get; set; }
        public double? NoOfBodhs { get; set; }
        public double? TotalWeight { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Amount { get; set; }
        public string? Mark { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string? PartyInvoiceNumber { get; set; }
        public decimal? Discount { get; set; }
        public decimal? NetAmount { get; set; }
        public decimal? Cess { get; set; }
        public decimal? SGST { get; set; }
        public decimal? CGST { get; set; }
        public decimal? IGST { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Createdby { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public decimal? SchemeDiscount { get; set; }
        public double? FreeQty { get; set; }
        public decimal? IGSTRate { get; set; }
        public decimal? SGSTRate { get; set; }
        public decimal? CGSTRate { get; set; }
        public decimal? CessRate { get; set; }
        public decimal? UnloadHamali { get; set; }
        public decimal? VikriFrieght { get; set; }
        public decimal? VikriCashAdvance { get; set; }
        public decimal? EmptyGunnybags { get; set; }
        public string? BuyerCode { get; set; }
        public int? ToPrint { get; set; }
        public decimal? Taxable { get; set; }
        public string? DisplayNo { get; set; }
        public string? DisplayinvNo { get; set; }
        public int? IsTender { get; set; }
        public int? IsTotalWeight { get; set; }
        public decimal? SellingRate { get; set; }
        public string? WeightInString { get; set; }
        public string? InputCode { get; set; }
        public string? InputLot { get; set; }
        public string? OutputCode { get; set; }
        public string? OutputLot { get; set; }
        public int? MonthNo { get; set; }
        public int? IsTaxableValueupdt { get; set; }
        public string? OriginalInvNo { get; set; }
        public int? AprReturned { get; set; }
        public int? ISUSED { get; set; }
        public DateTime? OriginalInvDate { get; set; }
        public decimal? Frieght { get; set; }
        public Boolean? IsActive { get; set; }
    }
}
