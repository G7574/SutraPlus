using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SutraPlus_DAL.Models
{
    public partial class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int _Id { get; set; }
        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }
        public string Place { get; set; } = null!;
        public string? Gstin { get; set; }
        public string? ContactDetails { get; set; }
        public string? Shree { get; set; }
        public string? KannadaName { get; set; }
        public string? Pan { get; set; }
        public string? FirmCode { get; set; }
        public string? Apmccode { get; set; }
        public string? Iec { get; set; }
        public string? Fln { get; set; }
        public string? Bin { get; set; }
        public string? Bank1 { get; set; }
        public string? Ifsc1 { get; set; }
        public string? AccountNo1 { get; set; }
        public string? Bank2 { get; set; }
        public string? Ifsc2 { get; set; }
        public string? AccountNo2 { get; set; }
        public string? Bank3 { get; set; }
        public string? Ifsc3 { get; set; }
        public string? Account3 { get; set; }
        public string? Title { get; set; }
        public string? District { get; set; }
        public string? State { get; set; }
        public string? Email { get; set; }
        public string? CellPhone { get; set; }
        public string? Tan { get; set; }
        public string? Cst { get; set; }
        public string? Tin { get; set; }
        public bool? StandardBilling { get; set; }
        public bool? AutoDeductTds { get; set; }
        public bool? CashEntrySystem { get; set; }
        public bool? RateInclusiveTax { get; set; }
        public bool? FarmerBill { get; set; }
        public bool? TraderBill { get; set; }
        public bool? PrintVochour { get; set; }
        public string? Sender { get; set; }
        //public string? UserName { get; set; }
        //public string? UserPass { get; set; }
        public string? SecondLineForReport { get; set; }
        public string? ThirdLineForReport { get; set; }
        public string? ReportTile1 { get; set; }
        public string? ReportTile2 { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public byte[]? Logo { get; set; }
        public DateTime? LastOpenend { get; set; }
        public int? BillNo { get; set; }
        public string? KannadaAddress { get; set; }
        public string? KannadaPlace { get; set; }
        public string? Jurisdiction { get; set; }
        public string? Fssai { get; set; }
        public int? OldFid { get; set; }
        public string? JurisLine { get; set; }
        //public string? RptTitle { get; set; }
        //  public string? HsnCode { get; set; }
        public string? NameColor { get; set; }
        public int? Printweights { get; set; }
        public int? Cp { get; set; }
        public string? Gid { get; set; }
        public string? Gpw { get; set; }
        public string? SelfEmailId { get; set; }
        public string? SelfWhatsUpNo { get; set; }
        public string? LicenceKey { get; set; }
        public int? WebId { get; set; }
        //public bool? QtyBilling { get; set; }
        //public int? IsTaxIncl { get; set; }
        public string? Lutno { get; set; }
        public int? Tcsreq { get; set; }
        public int? TcsreqinReceipt { get; set; }
        public int? VrtinForm { get; set; }
        public string? BillCode { get; set; }
        public string? PrintNumber { get; set; }
        public int? DirectPrint { get; set; }
        public int? NoOfCopies { get; set; }
        public int? DeleteWeight { get; set; }
        public string? EinvoiceKey { get; set; }
        public string? EinvoiceSkey { get; set; }
        public string? EinvoiceUserName { get; set; }
        public string? EinvoicePassword { get; set; }
        public int? DontTaxFrieght { get; set; }
        public int? EinvoiceReq { get; set; }
        public string? Pin { get; set; }
        public string? LegalName { get; set; }
        public string? PortaluserName { get; set; }
        public string? PortalPw { get; set; }
        public string? PortalEmail { get; set; }
        public string? NeftAcno { get; set; }
        public int? VochNo { get; set; }
        public int? VochType { get; set; }
        public int? LedgerId { get; set; }
        public string? InvType { get; set; }
        public int? Iscrystal { get; set; }
        public string? ReportName { get; set; }
        public string? Dbname { get; set; }
        public int? IsStandard { get; set; }
        public int? AskTcs { get; set; }
        public int? AskPdf { get; set; }
        public int? IsShopSent { get; set; }
        public string? HexCode { get; set; }
        public string? CustomerId { get; set; }
        public int? IsVps { get; set; }
        public int? Setcmdac { get; set; }
        public int? AddColumns1 { get; set; }
        public int? AskEinvoice { get; set; }
        public int? IsDuplicateDeleted { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int? IsVikriReq { get; set; }
        public int? AutoTds { get; set; }
        public int? Tcstotaxable { get; set; }
        public int? AskOnlineBackUpe { get; set; }
        public string? PhyPath { get; set; }
        public int? DeleteEx { get; set; }
        public int? ResetPacking { get; set; }
        public Boolean IsActive { get; set; }
        public string InvoiceString { get; set; }
    }
}
