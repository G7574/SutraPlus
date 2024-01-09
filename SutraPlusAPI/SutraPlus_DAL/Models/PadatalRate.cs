using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class PadatalRate
    {
        public decimal Packing { get; set; }
        public decimal Hamali { get; set; }
        public decimal WeighmanFee { get; set; }
        public decimal BazarCommission { get; set; }
        public decimal Cess { get; set; }
        public decimal Sgst { get; set; }
        public decimal Cgst { get; set; }
        public decimal PurchseCommission { get; set; }
        public decimal CartageRate { get; set; }
        public decimal RefillRate { get; set; }
        public decimal RepairRate { get; set; }
    }
}
