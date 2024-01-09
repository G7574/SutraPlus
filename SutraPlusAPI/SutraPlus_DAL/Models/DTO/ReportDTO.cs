namespace SutraPlus_DAL.Models.DTO
{
    public class ReportDTO
    {
        public long? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string Place { get; set; }
        public string GSTIN { get; set; }

        public long? VochType { get; set; }
        public long? VochNo { get; set; }
        public DateTime? TranctDate { get; set; }
        public double? NoOfBags { get; set; }
        public double? TotalWeight { get; set; }
        public decimal? Amount { get; set; }
        public string PartyInvoiceNumber { get; set; }
        public decimal? SGST { get; set; }
        public decimal? CGST { get; set; }
        public decimal? IGST { get; set; }
        public decimal? Taxable { get; set; }
        public int? ToPrint { get; set; }

        public long? CommodityId { get; set; }
        public string CommodityName { get; set; }
        public string HSN { get; set; }

        public string LedgerName { get; set; }
        public string Ledger_Place { get; set; }
        public string Ledger_GSTIN { get; set; }

        public long? VoucherId { get; set; }
        public string VoucherName { get; set; }

        /// <summary>
        /// Monthwise report
        /// </summary>
        public string? MonthNo { get; set;}
        public decimal? BasicValue { get; set;}
        public decimal? Others { get; set; }
        public decimal? BillAmount { get; set; }


        /// <summary>
        /// Trail Balance
        /// </summary>
        public int? AccountingGroupId { get; set; }
        public string? GroupName { get; set; }
        public string? OpeningBalance { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Debit { get; set; }

        ///
        ///Payment List
        ///
        public decimal? YadiBalance { get; set; }
        public decimal? AccountBalance { get; set; }
        public DateTime? AsOnDate { get; set; }

        ///
        /// PartywiseTDS
        /// 
        public string? PAN { get; set; }
        public string? Ledger_PAN { get; set; }
        public decimal? TotalCommission { get; set; }
        public decimal? TDSDeducted { get; set; }
        public decimal? CommissionTDS { get; set; }


        ///
        ///Transactin Summary
        ///
        public string? Gstn { get; set; }
        
    }

}
