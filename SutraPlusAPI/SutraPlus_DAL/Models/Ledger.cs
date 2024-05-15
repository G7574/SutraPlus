using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SutraPlus_DAL.Models
{
    public partial class Ledger
    {
        [Key]           
        public int _Id { get; set; }
        public long? CompanyId { get; set; }
        public string? LedgerType { get; set; }
        public long? LedgerId { get; set; }
        public string? LedgerName { get; set; } 
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Place { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Gstin { get; set; }
        public string? DealerType { get; set; }
        public string? KannadaName { get; set; }
        public string? KannadaPlace { get; set; }
        public string? LedgerCode { get; set; }
        public string? ContactDetails { get; set; }
        public string? Pan { get; set; }
        public string? BankName { get; set; }
        public string? Ifsc { get; set; }
        public string? AccountNo { get; set; }
        public int? AccountingGroupId { get; set; }
        public string? NameAndPlace { get; set; }
        public decimal? PackingRate { get; set; }
        public decimal? HamaliRate { get; set; }
        public decimal? WeighManFeeRate { get; set; }
        public decimal? DalaliRate { get; set; }
        public decimal? CessRate { get; set; }
        public decimal? DeprPerc { get; set; }
        public decimal? CaplPerc { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? RenewDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public string? CellNo { get; set; }
        public int? OtherCreated { get; set; }
        public string? LocalName { get; set; }
        public string? LocalAddress { get; set; }
        public decimal? UnloadHamaliRate { get; set; }
        public long? OldHdid { get; set; }
        public string? Gstn { get; set; }
        public long? Dlr_Type { get; set; }
        public int? Tp { get; set; }
        public int? Ist { get; set; }
        public string? Bname { get; set; }
        public string? EmailId { get; set; }
        public int? PrintAcpay { get; set; }
        public int? ExclPay { get; set; }
        public long? OldLedgerId { get; set; }
        public string? AgentCode { get; set; }
        public string? Pin { get; set; }
        public string? StateCode { get; set; }
        public string? LegalName { get; set; }
        public string? NeftAcno { get; set; }
        public string? ChequeNo { get; set; }
        public int? AskIndtcs { get; set; }
        public int? CommodityAccount { get; set; }
        public int? ApplyTds { get; set; }
        public string? Fssai { get; set; }
        public int? Dperc { get; set; }
        public decimal? RentTdsperc { get; set; }
        public int? IsSelected { get; set; }
        public int? ToPrint { get; set; }
        public decimal? TotalCommission { get; set; }
        public decimal? Tdsdeducted { get; set; }
        public int? IsExported { get; set; }
        public int? DeductFrieghtTds { get; set; }
        public decimal? TotalForTds2 { get; set; }
        public decimal? Tds2deducted { get; set; }
        public int? ManualBookPageNo { get; set; }
        public decimal? QtoBeDeducted { get; set; }
        public decimal? Qtdsdeducted { get; set; }
        public decimal? TotalTv { get; set; }
        public decimal? TotalTurnoverforTcs { get; set; }
        public decimal? Tcsdeducted { get; set; }
        public decimal? TcstoBeDeducted { get; set; }
        public decimal? BalanceTcs { get; set; }
        public decimal? BalanceQtds { get; set; }
        public decimal? RentToBeDeducted { get; set; }
        public decimal? RentTdsdeducted { get; set; }
        public decimal? BalanceRentTds { get; set; }
        public decimal? ClosingBalanceCr { get; set; }
        public decimal? TotalTransaction { get; set; }
        public decimal? ClosingBalanceDr { get; set; }
        public decimal? TotalContactTv { get; set; }
        public decimal? OpeningBalance { get; set; }
        public string? CrDr { get; set; }
        public int? IsActive { get; set; }
        public string? LedType { get; set; }
    }
}
