using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SutraPlus_DAL.Models
{
    public partial class Commodity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int _Id { get; set; }
        public long? CommodityId { get; set; }
        public string? CommodityName { get; set; }
        public long? CompanyId { get; set; }
        public string? Mou { get; set; }
        public decimal? IGST { get; set; }
        public decimal? SGST { get; set; }
        public string? HSN { get; set; }
        public decimal? CGST { get; set; }
        public bool? IsTrading { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public string? Category { get; set; }
        public double? OpeningStock { get; set; }
        public int? PcsPerBox { get; set; }
        public decimal? RecentPurchaseRate { get; set; }
        public int? MinStockLevel { get; set; }
        public decimal? Cess { get; set; }
        public decimal? Mrp { get; set; }
        public decimal? SellingRate { get; set; }
        public string? BrandName { get; set; }
        public string? LocalName { get; set; }
        public bool? IsVikryCommodity { get; set; }
        public bool? IsVikriCommodity { get; set; }
        public decimal? Obval { get; set; }
        public string? BarCode { get; set; }
        public bool? DeductTds { get; set; }
        public string? Sno { get; set; }
        public int? ToPrint { get; set; }
        public string? SearchName { get; set; }
        public decimal? ShortageWeight { get; set; }
        public decimal? ClosingWeight { get; set; }
        public decimal? AvaregeRate { get; set; }
        public decimal? ClosingValue { get; set; }
        public decimal? OutLetId { get; set; }
        public bool? IsService { get; set; }
        public bool IsActive { get; set; }
    }
}
