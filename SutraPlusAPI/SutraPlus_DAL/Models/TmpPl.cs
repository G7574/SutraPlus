using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class TmpPl
    {
        public int? CompanyId { get; set; }
        public string? ItemName { get; set; }
        public string? PurchaseParticulars { get; set; }
        public double? PurchaseWeight { get; set; }
        public decimal? PurchaseAmount { get; set; }
        public decimal? Total { get; set; }
        public string? SaleParticulars { get; set; }
        public double? SaleWeight { get; set; }
        public decimal? SaleAmount { get; set; }
    }
}
