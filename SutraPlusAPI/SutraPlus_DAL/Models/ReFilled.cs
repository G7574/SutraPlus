using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class ReFilled
    {
        public string? PadatalName { get; set; }
        public int NoOfBags { get; set; }
        public double BagWeight { get; set; }
        public double Weight { get; set; }
    }
}
