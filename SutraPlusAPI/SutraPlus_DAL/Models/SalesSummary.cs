using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class SalesSummary
    {
        public long CompanyId { get; set; }
        public long VochType { get; set; }
        public long VochNo { get; set; }
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
        public decimal? Aredate { get; set; }
        public DateTime? Areno { get; set; }
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
        public string? Inwords { get; set; }
        public decimal? FrieghtAmount { get; set; }
        public decimal? RoundOff { get; set; }
    }
}
