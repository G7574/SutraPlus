using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SutraPlus_DAL.Models
{
    public class SalesInvoice
    {
        //[Required]
        //[ForeignKey("_Id")]

        //public BillSummary billSummary { get; set; }
        //public Inventory inventory { get; set; }    
        public int _Id { get; set; }
        public long? CompanyId { get; set; }
        public long? LedgerId { get; set; }
        public long? VochType { get; set; }
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


        //public int _Id { get; set; }
        //public long? CompanyId { get; set; }
        //public long? VochType { get; set; }
        //public long? VochNo { get; set; }
        //public long? LedgerId { get; set; }
        public long? CommodityId { get; set; }
        //public DateTime? TranctDate { get; set; }
        public string? PoNumber { get; set; }
        public string? EwaybillNo { get; set; }
        public long? LotNo { get; set; }
        public double? WeightPerBag { get; set; }
        public double? NoOfBags { get; set; }
        public double? NoOfDocra { get; set; }
        public double? NoOfBodhs { get; set; }
      //  public double? TotalWeight { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Amount { get; set; }
        public string? Mark { get; set; }
        public long Id { get; set; }
       // public string? PartyInvoiceNumber { get; set; }
     //   public decimal? Discount { get; set; }
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
       // public decimal? VikriFrieght { get; set; }
        //public decimal? VikriCashAdvance { get; set; }
        public decimal? EmptyGunnybags { get; set; }
        public string? BuyerCode { get; set; }
       // public int? ToPrint { get; set; }
        public decimal? Taxable { get; set; }
        //public string? DisplayNo { get; set; }
        //public string? DisplayinvNo { get; set; }
        public int? IsTender { get; set; }
        public int? IsTotalWeight { get; set; }
        public decimal? SellingRate { get; set; }
        //public string? WeightInString { get; set; }
        public string? InputCode { get; set; }
        public string? InputLot { get; set; }
        public string? OutputCode { get; set; }
        public string? OutputLot { get; set; }
        public int? MonthNo { get; set; }
        public int? IsTaxableValueupdt { get; set; }
      ///  public string? OriginalInvNo { get; set; }
        public int? AprReturned { get; set; }
        public int? ISUSED { get; set; }
        //public DateTime? OriginalInvDate { get; set; }
        public decimal? Frieght { get; set; }
        //public Boolean? IsActive { get; set; }
    }
}
