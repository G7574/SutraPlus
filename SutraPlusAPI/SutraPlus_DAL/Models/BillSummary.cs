using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SutraPlus_DAL.Models
{
    [Table("BillSummary")]
    public partial class BillSummary
    {
    
        
        public int? _Id { get; set; }
        public long? CompanyId { get; set; }
        public long? LedgerId { get; set; }
        public string? LedgerName { get; set; }
        public string? Place { get; set; }
        public long? VochType { get; set; }
        public string? VoucherName { get; set; }
        public string? DealerType { get; set; } // new field added
        public string? PAN { get; set; } // new field added
        public string? GST { get; set; } // new field added
        public string? State { get; set; } // new field added
        public string? InvoiceType { get; set; } //IMP for different invoice type pass (SalesGood, SalesExport, SalesGinning) value from UI to differentiate
        public long? VochNo { get; set; }
        public string? EwayBillNo { get; set; }
        public string? Ponumber { get; set; }
        public string? Transporter { get; set; }
        public string? LorryNo { get; set; }
        public string? LorryOwnerName { get; set; }
        public string? DriverName { get; set; }
        public string? Dlno { get; set; }
        public string? CheckPost { get; set; }
        public decimal? FrieghtPerBag { get; set; }
        public decimal? TotalFrieght { get; set; }
        public decimal? Advance { get; set; }
        public decimal? Balance { get; set; }
        public string? AREDate { get; set; }
        public string? ARENo { get; set; }
        public bool? IsLessOrPlus { get; set; }
        public string? ExpenseName1 { get; set; }
        public string? ExpenseName2 { get; set; }
        public string? ExpenseName3 { get; set; }
        public decimal? ExpenseAmount1 { get; set; }
        public decimal? ExpenseAmount2 { get; set; }
        public decimal? ExpenseAmount3 { get; set; }
        public string? DeliveryName { get; set; }
        public string? DeliveryAddress1 { get; set; }
        public string? DeliveryAddress2 { get; set; }
        public string? DeliveryPlace { get; set; }
        public string? DeliveryState { get; set; }
        public string? DeliveryStateCode { get; set; }
        public decimal? BillAmount { get; set; }
        public string? INWords { get; set; }
        public decimal? FrieghtAmount { get; set; }
        public decimal? RoundOff { get; set; }
        public long? TotalBags { get; set; }
        public double? TotalWeight { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? PackingValue { get; set; }
        public decimal? HamaliValue { get; set; }
        public decimal? WeighnamFeeValue { get; set; }
        public decimal? DalaliValue { get; set; }
        public decimal? CessValue { get; set; }
        public decimal? TaxableValue { get; set; }
        public decimal? SGSTValue { get; set; }
        public decimal? CSGSTValue { get; set; }
        public decimal? IGSTValue { get; set; }
        public int? UserId { get; set; }
        public int? SessionID { get; set; }
        public string? PaymentBy { get; set; }
        public decimal? AmountReceived { get; set; }
        public decimal? Change { get; set; }
        public string? CardDetails { get; set; }
        public DateTime? BillTime { get; set; }
        public DateTime? TranctDate { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPlace { get; set; }
        public string? CustomerContactNo { get; set; }
        public string? PartyInvoiceNumber { get; set; }
        public int? OtherCreated { get; set; }
        public int? Recent { get; set; }
        public int? PaymentReceived { get; set; }
        public string? TagName { get; set; }
        public DateTime? TagDate { get; set; }
        public int? ToPrint { get; set; }
        public int? frieghtPlus { get; set; }
        public string? StateCode1 { get; set; }
        public string? StateCode2 { get; set; }
        public decimal? ExpenseTax { get; set; }
        public decimal? FrieghtinBill { get; set; }
        public DateTime? LastOpenend { get; set; }
        public decimal? VikriCommission { get; set; }
        public decimal? VikriULH { get; set; }
        public decimal? VikriCashAdvance { get; set; }
        public decimal? VikriFrieght { get; set; }
        public decimal? VikriEmpty { get; set; }
        public decimal? VikriNet { get; set; }
        public decimal? CashToFarmer { get; set; }
        public decimal? VikriOther1 { get; set; }
        public decimal? VikriOther2 { get; set; }
        public string? VikriOther1Name { get; set; }
        public string? VikriOther2Name { get; set; }
        public string? Note1 { get; set; }
        public string? FromPlace { get; set; }
        public string? ToPlace { get; set; }
        public string? DisplayNo { get; set; }
        public string? DisplayinvNo { get; set; }
        public string? FrieghtLabel { get; set; }
        public string? FormName { get; set; }
        public string? DCNote { get; set; }
        public string? WeightInString { get; set; }
        public string? SGSTLabel { get; set; }
        public string? CGSTLabel { get; set; }
        public string? IGSTLabel { get; set; }
        public string? TCSLabel { get; set; }
        public decimal? tdsperc { get; set; }
        public decimal? TCSValue { get; set; }
        public decimal? TCSPerc { get; set; }
        public string? DelPinCode { get; set; }
        public int? CommodityID { get; set; }
        public byte[]? QrCode { get; set; }
        public int? IsGSTUpload { get; set; }
        public string? DispatcherName { get; set; }
        public string? DispatcherAddress1 { get; set; }
        public string? DispatcherAddress2 { get; set; }
        public string? DispatcherPlace { get; set; }
        public string? DispatcherPIN { get; set; }
        public string? DispatcherStatecode { get; set; }
        public string? CountryCode { get; set; }
        public string? ShipBillNo { get; set; }
        public string? ForCur { get; set; }
        public string? PortName { get; set; }
        public string? RefClaim { get; set; }
        public DateTime? ShipBillDate { get; set; }
        public string? ExpDuty { get; set; }
        public int? monthNo { get; set; }
        public int? Distance { get; set; }
        public string? GSTR1SectionName { get; set; }
        public string? GSTR1InvoiceType { get; set; }
        public string? EInvoiceNo { get; set; }
        public int? IsSEZ { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? id { get; set; }
        public string? OriginalInvNo { get; set; }
        public DateTime? OriginalInvDate { get; set; }
        public int? AprClosed { get; set; }
        public decimal? Discount { get; set; }
        public decimal? TwelveValue { get; set; }
        public decimal? FiveValue { get; set; }
        public decimal? EgthValue { get; set; }
        public string? ACKNO { get; set; }
        public string? IRNNO { get; set; }
        public string? SignQRCODE { get; set; }
        public Boolean? IsActive { get; set; }
        public Boolean? IsServiceInvoice { get; set; }
        
    }
}
