using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class TmpStockLedger
    {
        public int CompanyId { get; set; }
        public DateTime TranctDate { get; set; }
        public double? Obstock { get; set; }
        public decimal? Obvalue { get; set; }
        public double? PurchaseQty { get; set; }
        public decimal PurchaseValue { get; set; }
        public double SalesReturnQty { get; set; }
        public decimal SalesReturnValue { get; set; }
        public decimal FromProductionQty { get; set; }
        public decimal? FromProductionValue { get; set; }
        public decimal OnwSalesQty { get; set; }
        public decimal? OnwSalesValue { get; set; }
        public decimal? ToProduction { get; set; }
        public decimal? ClosingStock { get; set; }
        public decimal? Average { get; set; }
        public decimal? ClosingValue { get; set; }
    }
}
