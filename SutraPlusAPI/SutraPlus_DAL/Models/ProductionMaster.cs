using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class ProductionMaster
    {
        public int Companyid { get; set; }
        public DateTime TranctDate { get; set; }
        public decimal? Refno { get; set; }
        public double QtySent { get; set; }
        public double QtyRcd { get; set; }
        public int SentItemid { get; set; }
        public int ReceivedItId { get; set; }
        public decimal GoodsValue { get; set; }
        public long SrNo { get; set; }
        public int ReceivedItemid { get; set; }
        public string ReceivedItemName { get; set; } = null!;
        public string SentItemName { get; set; } = null!;
    }
}
