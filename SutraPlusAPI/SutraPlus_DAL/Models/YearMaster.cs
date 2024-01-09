using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class YearMaster
    {
        public int Id { get; set; }
        public string? Year { get; set; }
        public string? FinYear { get; set; }
        public Boolean IsActive { get; set; }
    }
}
