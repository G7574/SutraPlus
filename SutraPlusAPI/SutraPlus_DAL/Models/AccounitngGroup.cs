using System;
using System.Collections.Generic;

namespace SutraPlus_DAL.Models
{
    public partial class AccounitngGroup
    {
        //public int _Id { get; set; }
        public long? AccontingGroupId { get; set; }
        public string? GroupName { get; set; } = null!;
        public string? UnderHead { get; set; }
        public string? LiaorAsset { get; set; }
        public double? PositionInBalanceSheet { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? PandLposition { get; set; }
        public int? CreditPosition { get; set; }
        public int? DebitPosition { get; set; }
        //public Boolean? IsActive { get; set; }
    }
}
