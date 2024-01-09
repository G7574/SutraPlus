using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class Summary
    {
        public int CompanyId { get; set; }
        public DateTime TranctDate { get; set; }
        public string PadatalName { get; set; } = null!;
        public long TotalBags { get; set; }
        public decimal Bastani { get; set; }
        public double TotalWeight { get; set; }
        public decimal PackingRate { get; set; }
        public decimal PackingValue { get; set; }
        public decimal HamaliRate { get; set; }
        public decimal HamaliValues { get; set; }
        public decimal WeighmanFeeRate { get; set; }
        public decimal WeighmanFeeValue { get; set; }
        public decimal BazarCommissionRate { get; set; }
        public decimal BazarCommissionValue { get; set; }
        public decimal CessRate { get; set; }
        public decimal CessValue { get; set; }
        public decimal TaxableValue { get; set; }
        public decimal Sgstrate { get; set; }
        public decimal Sgstvalue { get; set; }
        public decimal Cgstrate { get; set; }
        public decimal Cgstvalue { get; set; }
        public decimal FirstRoundOff { get; set; }
        public decimal BillTotal { get; set; }
        public decimal PurchaseCommissionRate { get; set; }
        public decimal PurchaseCommissionValues { get; set; }
        public decimal SecondRoundOff { get; set; }
        public decimal GrandToal { get; set; }
        public decimal AverageRate { get; set; }
        public decimal NoofEmptyBags { get; set; }
        public decimal BagRate { get; set; }
        public decimal BagAmount { get; set; }
        public decimal StemlessCharges { get; set; }
        public decimal GrindingCharges { get; set; }
        public decimal CrushingCharges { get; set; }
        public decimal LorryFrieght { get; set; }
        public decimal Shortage { get; set; }
        public decimal PadatalRate { get; set; }
        public decimal CartageValue { get; set; }
        public decimal Refillvalue { get; set; }
        public decimal RepairValue { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal LooseRate { get; set; }
        public int? ToPrint { get; set; }
        public string? Refilled1 { get; set; }
        public string? Refilled2 { get; set; }
        public string? Refilled3 { get; set; }
        public string? Refilled4 { get; set; }
        public string? Refilled5 { get; set; }
        public string? Refilled6 { get; set; }
        public string? Refilled7 { get; set; }
        public string? Expense1 { get; set; }
        public decimal? Expense1Amount { get; set; }
        public string? Expense2 { get; set; }
        public decimal? Expense2Amount { get; set; }
        public string? Expense3 { get; set; }
        public decimal? Expense3Amount { get; set; }
        public string? Expense4 { get; set; }
        public decimal? Expense4Amount { get; set; }
        public string? CommissionTo { get; set; }
    }
}
